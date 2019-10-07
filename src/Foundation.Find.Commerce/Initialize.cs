using EPiServer.Find.Commerce;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Foundation.Find.Commerce.ViewModels;
using System.Web.Mvc;

namespace Foundation.Find.Commerce
{
    [ModuleDependency(typeof(Cms.Initialize), typeof(FindCommerceInitializationModule))]
    public class Initialize : IConfigurableModule
    {
        void IConfigurableModule.ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddSingleton<CatalogContentClientConventions, FoundationFindConventions>();
            services.AddTransient<IModelBinderProvider, FindCommerceModelBinderProvider>();
            services.AddTransient<ICommerceSearchService, CommerceSearchService>();
            services.AddSingleton<CatalogContentEventListener, FoundationCatalogContentEventListener>();
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }
    }
}