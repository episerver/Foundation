using EPiServer.Forms.Core;
using EPiServer.Forms.Core.Internal.ExternalSystem;
using Episerver.Marketing.Connector.Forms;
using Episerver.Marketing.Connector.Forms.Repositories;
using Episerver.Marketing.Connector.Framework;
using Episerver.Marketing.Connector.Framework.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Infrastructure;

/*
 *  Copy this file into your CMS Project and 
 *  Add the following in Startup.cs if using Marketing Automation Connectors
 *         services.AddFormRepositoryWorkAround();
 */
public class CustomFormsRepository : IFormsRepository
{
    private readonly IServiceProvider _serviceProvider;
    private readonly IFormsRepository _instance;

    public CustomFormsRepository(IServiceProvider serviceProvider, IFormsRepository instance)
    {
        _serviceProvider = serviceProvider;
        _instance = instance;
    }

    public bool IsAutoFillEnabled()
    {
        return _instance.IsAutoFillEnabled();
    }

    public IEnumerable<string> GetSuggestedFormValues(IDatasource selectedDatasource, IEnumerable<RemoteFieldInfo> remoteFieldInfos,
        ElementBlockBase content, IFormContainerBlock formContainerBlock, HttpContext context)
    {
        if (!IsAutoFillEnabled() || !selectedDatasource.Id.Contains(FormsConstants.DataSourceIdListSeparator))
            return new List<string>();

        return _instance.GetSuggestedFormValues(selectedDatasource, remoteFieldInfos, content, formContainerBlock,
            context);
    }

    public void PushDataToConnector(IMarketingConnector currentConnector, ConnectorDataSource currentDatasource,
        Dictionary<string, string> entityData, long submissionTarget)
    {
        _instance.PushDataToConnector(currentConnector, currentDatasource, entityData, submissionTarget);
    }

    public Dictionary<string, string> GetConnectorMappedData(IEnumerable<KeyValuePair<string, RemoteFieldInfo>> submittedFieldMappingTable, IDictionary<string, object> submittedData)
    {
        return _instance.GetConnectorMappedData(submittedFieldMappingTable, submittedData);
    }

    public string ConvertGuidToAdaptedGuid(Guid theGuid)
    {
        return _instance.ConvertGuidToAdaptedGuid(theGuid);
    }

    public Guid ConvertAdaptedGuidToGuid(string theAdaptedGuid)
    {
        return _instance.ConvertAdaptedGuidToGuid(theAdaptedGuid);
    }
}

public static class FormRepositoryHelper
{
    public static IServiceCollection AddFormRepositoryWorkAround(this IServiceCollection services)
    {
        return services.Intercept<IFormsRepository>((a, b) => new CustomFormsRepository(a, b));
    }
} 