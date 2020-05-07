using EPiServer.Commerce.Internal.Migration;
using EPiServer.ContentApi.Core.Configuration;
using EPiServer.ContentApi.Search;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Foundation.Cms.Extensions;
using Foundation.Commerce.Extensions;
using Foundation.Demo.Extensions;
using Foundation.Find.Cms;
using Foundation.Infrastructure.Services;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Owin;

namespace Foundation.Infrastructure
{
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    [ModuleDependency(typeof(Cms.Initialize))]
    [ModuleDependency(typeof(EPiServer.ServiceApi.IntegrationInitialization))]
    [ModuleDependency(typeof(EPiServer.ContentApi.Core.Internal.ContentApiCoreInitialization))]
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    public class InitializeSite : IConfigurableModule
    {
        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            context.ConfigureFoundationCms();
            context.Services.Configure<ContentApiConfiguration>(c =>
            {
                c.EnablePreviewFeatures = true;
                c.Default(RestVersion.Version_3_0)
                    .SetMinimumRoles(string.Empty)
                    .SetRequiredRole(string.Empty);
                c.Default(RestVersion.Version_2_0)
                    .SetMinimumRoles(string.Empty)
                    .SetRequiredRole(string.Empty);
            });

            context.Services.Configure<ContentApiSearchConfiguration>(config =>
            {
                config.Default()
                .SetMaximumSearchResults(200)
                .SetSearchCacheDuration(TimeSpan.FromMinutes(60));
            });

            context.Services.AddSingleton<ICampaignService, CampaignService>();
        }

        public void Initialize(InitializationEngine context)
        {
            var manager = context.Locate.Advanced.GetInstance<MigrationManager>();
            if (manager.SiteNeedsToBeMigrated())
            {
                manager.Migrate();
            }

            context.InitializeFoundationCms();
            context.InitializeFoundationCommerce();
            context.InitializeFoundationFindCms();
            context.InitializeFoundationDemo();


            var handler = GlobalConfiguration.Configuration.MessageHandlers
                .FirstOrDefault(x => x.GetType() == typeof(PassiveAuthenticationMessageHandler));

            if (handler != null)
            {
                GlobalConfiguration.Configuration.MessageHandlers.Remove(handler);
            }

            //GlobalConfiguration.Configuration.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;
        }

        public void Uninitialize(InitializationEngine context)
        {

        }
    }
}