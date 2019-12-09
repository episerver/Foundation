using EPiServer.Find.Commerce;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ContentQuery;
using Foundation.Find.Cms.Facets.Config;
using Foundation.Find.Commerce.Facets;
using Foundation.Find.Commerce.PowerSlices;
using Foundation.Find.Commerce.ViewModels;
using PowerSlice;
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
            services.AddTransient<IContentQuery, ProductsSlice>();
            services.AddTransient<IContentSlice, ProductsSlice>();
            services.AddTransient<IContentQuery, PackagesSlice>();
            services.AddTransient<IContentSlice, PackagesSlice>();
            services.AddTransient<IContentQuery, BundlesSlice>();
            services.AddTransient<IContentSlice, BundlesSlice>();
            services.AddTransient<IContentQuery, VariantsSlice>();
            services.AddTransient<IContentSlice, VariantsSlice>();
            services.AddTransient<IContentQuery, OrderPromotionsSlice>();
            services.AddTransient<IContentSlice, OrderPromotionsSlice>();
            services.AddTransient<IContentQuery, ShippingPromotionsSlice>();
            services.AddTransient<IContentSlice, ShippingPromotionsSlice>();
            services.AddTransient<IContentQuery, EntryPromotionsSlice>();
            services.AddTransient<IContentSlice, EntryPromotionsSlice>();
            services.AddSingleton<IFacetConfigFactory, CommerceFacetConfigFactory>();
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }
    }
}