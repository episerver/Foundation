using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Foundation.Infrastructure.Find.Facets;
using Foundation.Infrastructure.Find.Facets.Config;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;

namespace Foundation.Infrastructure.Find
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