using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Foundation.Find.Facets;
using Foundation.Find.Facets.Config;
using System.Collections.Generic;

namespace Foundation.Find
{
    [ModuleDependency(typeof(Cms.Initialize))]
    public class Initialize : IConfigurableModule
    {
        void IConfigurableModule.ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddSingleton<IFacetRegistry>(new FacetRegistry(new List<FacetDefinition>()));
            services.AddSingleton<IFacetConfigFactory, FacetConfigFactory>();
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }
    }
}