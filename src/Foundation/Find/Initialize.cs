using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Foundation.Find.Facets;
using Foundation.Find.Facets.Config;

namespace Foundation.Find
{
    [ModuleDependency(typeof(Cms.Initialize))]
    public class Initialize : IConfigurableModule
    {
        void IConfigurableModule.ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddSingleton<IFacetConfigFactory, FacetConfigFactory>();
            services.AddSingleton<IFacetRegistry>((locator) => new FacetRegistry(locator.GetInstance<IFacetConfigFactory>().GetDefaultFacetDefinitions()));
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }
    }
}