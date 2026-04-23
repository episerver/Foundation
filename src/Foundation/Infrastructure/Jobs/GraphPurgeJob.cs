using EPiServer.PlugIn;
using EPiServer.Scheduler;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Optimizely.Graph.Core.Configuration;
using System.Threading;
using System.Threading.Tasks;

namespace Foundation.Infrastructure.Jobs
{
    /// <summary>
    /// Purges all content types and data from the Optimizely Graph source for this site.
    ///
    /// Run this job when the "Optimizely Graph Full synchronization" job fails with:
    ///   UPDATE_CONTENT_LANG_ERROR — "Content type languages could not be updated"
    ///
    /// This typically happens when content types have changed (new types, modified properties,
    /// new languages) since the last successful sync, leaving the Graph source in an
    /// inconsistent state. Purging clears it and allows a clean Full Sync.
    ///
    /// Steps:
    ///   1. Run this job (Scheduled Jobs → Graph - Purge Source)
    ///   2. Run "Optimizely Graph Full synchronization" job
    /// </summary>
    [ScheduledPlugIn(
        DisplayName = "Graph - Purge Source (run before Full Sync on schema errors)",
        Description = "Purges content types and all indexed data from the Optimizely Graph source. Run this when Full Sync fails with UPDATE_CONTENT_LANG_ERROR, then run Full Sync again.",
        GUID = "d7e3a1b4-9c5f-4e82-a031-7f6c2d8e0b5a")]
    [ServiceConfiguration]
    public class GraphPurgeJob : ScheduledJobBase
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<GraphOptions> _graphOptions;

        public GraphPurgeJob(IServiceProvider serviceProvider, IOptions<GraphOptions> graphOptions)
        {
            _serviceProvider = serviceProvider;
            _graphOptions = graphOptions;
        }

        public override string Execute()
        {
            var source = _graphOptions.Value.AppKey;

            if (string.IsNullOrEmpty(source))
            {
                return "ERROR: AppKey is not configured in Optimizely:ContentGraph. Check appsettings.json.";
            }

            // ISyncClient is declared internal in Optimizely.Graph.Cms.
            // Locate the assembly by name (it is loaded because AddContentGraph registers from it)
            // and resolve the type via reflection to work around the internal visibility.
            var graphCmsAssembly = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(a => a.GetName().Name == "Optimizely.Graph.Cms")
                ?? throw new InvalidOperationException("Optimizely.Graph.Cms assembly is not loaded.");
            var syncClientType = graphCmsAssembly.GetType("Optimizely.Graph.Cms.Client.ISyncClient")
                ?? throw new InvalidOperationException("ISyncClient type not found in Optimizely.Graph.Cms assembly.");

            var syncClient = _serviceProvider.GetRequiredService(syncClientType);

            OnStatusChanged($"Checking if Graph source '{source}' exists...");

            var checkMethod = syncClientType.GetMethod("CheckSourceExistsAsync",
                new[] { typeof(string), typeof(CancellationToken) })
                ?? throw new InvalidOperationException("CheckSourceExistsAsync not found on ISyncClient.");

            var checkTask = (Task<bool>)checkMethod.Invoke(syncClient, new object[] { source, CancellationToken.None });
            var exists = checkTask.GetAwaiter().GetResult();

            if (!exists)
            {
                return $"Graph source '{source}' does not exist — nothing to purge. Run Full Sync to create it.";
            }

            OnStatusChanged($"Purging Graph source '{source}' (content types + data)...");

            var purgeMethod = syncClientType.GetMethod("PurgeContentTypesAndDataAsync",
                new[] { typeof(string), typeof(bool), typeof(CancellationToken) })
                ?? throw new InvalidOperationException("PurgeContentTypesAndDataAsync not found on ISyncClient.");

            var purgeTask = (Task)purgeMethod.Invoke(syncClient, new object[] { source, false, CancellationToken.None });
            purgeTask.GetAwaiter().GetResult();

            return $"Graph source '{source}' purged successfully. Now run 'Optimizely Graph Full synchronization' to rebuild the index.";
        }
    }
}
