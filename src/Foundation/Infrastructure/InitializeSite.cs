using EPiBootstrapArea;
using EPiBootstrapArea.Initialization;
using EPiServer.Commerce.Internal.Migration;
using EPiServer.Commerce.Order;
using EPiServer.ContentApi.Core.Configuration;
using EPiServer.ContentApi.Search;
using EPiServer.Find.ClientConventions;
using EPiServer.Find.Commerce;
using EPiServer.Find.Framework;
using EPiServer.Find.UnifiedSearch;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ContentQuery;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.PageExtensions;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Cms.Extensions;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.GiftCard;
using Foundation.Features.Blog.BlogItemPage;
using Foundation.Features.CatalogContent;
using Foundation.Features.CatalogContent.Product;
using Foundation.Features.CatalogContent.Services;
using Foundation.Features.Checkout;
using Foundation.Features.Checkout.Payments;
using Foundation.Features.Checkout.Services;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.Header;
using Foundation.Features.Home;
using Foundation.Features.Locations.LocationItemPage;
using Foundation.Features.Locations.LocationListPage;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyAccount.Bookmarks;
using Foundation.Features.MyAccount.CreditCard;
using Foundation.Features.MyOrganization;
using Foundation.Features.MyOrganization.Budgeting;
using Foundation.Features.MyOrganization.Organization;
using Foundation.Features.Search;
using Foundation.Features.Shared;
using Foundation.Features.Stores;
using Foundation.Find;
using Foundation.Infrastructure.Display;
using Foundation.Infrastructure.PowerSlices;
using Foundation.Infrastructure.SchemaMarkup;
using Foundation.Infrastructure.Services;
using PowerSlice;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Owin;
using System.Web.Mvc;

namespace Foundation.Infrastructure
{
    [ModuleDependency(typeof(EPiServer.Commerce.Initialization.InitializationModule))]
    [ModuleDependency(typeof(Cms.Initialize))]
    [ModuleDependency(typeof(EPiServer.ServiceApi.IntegrationInitialization))]
    [ModuleDependency(typeof(EPiServer.ContentApi.Core.Internal.ContentApiCoreInitialization))]
    [ModuleDependency(typeof(ServiceContainerInitialization))]
    [ModuleDependency(typeof(SetupBootstrapRenderer))]
    public class InitializeSite : IConfigurableModule
    {
        private IServiceConfigurationProvider _services;

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            _services = context.Services;
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

            context.Services.AddSingleton<IDisplayModeFallbackProvider, FoundationDisplayModeProvider>();
            context.Services.AddTransient<IQuickNavigatorItemProvider, FoundationQuickNavigatorItemProvider>();
            context.Services.AddTransient<IViewTemplateModelRegistrator, ViewTemplateModelRegistrator>();
            context.Services.AddSingleton<DefaultPlacedPriceProcessor, FoundationPlacedPriceProcessor>();
            context.Services.AddSingleton<ISearchViewModelFactory, SearchViewModelFactory>();
            context.Services.AddSingleton<IPaymentService, PaymentService>();
            context.Services.AddTransient<CheckoutViewModelFactory>();
            context.Services.AddSingleton<MultiShipmentViewModelFactory>();
            context.Services.AddSingleton<OrderSummaryViewModelFactory>();
            context.Services.AddTransient<PaymentMethodViewModelFactory>();
            context.Services.AddSingleton<IBookmarksService, BookmarksService>();
            context.Services.AddSingleton<IPricingService, PricingService>();
            context.Services.AddSingleton<IB2BNavigationService, B2BNavigationService>();
            context.Services.AddSingleton<IBudgetService, BudgetService>();
            context.Services.AddSingleton<ICreditCardService, CreditCardService>();
            context.Services.AddSingleton<IGiftCardService, GiftCardService>();
            context.Services.AddSingleton<IOrganizationService, OrganizationService>();
            context.Services.AddSingleton<IQuickOrderService, QuickOrderService>();
            context.Services.AddSingleton<IProductService, ProductService>();
            context.Services.AddSingleton<IPromotionService, PromotionService>();
            context.Services.AddSingleton<IStoreService, StoreService>();
            context.Services.AddSingleton<CatalogEntryViewModelFactory>();
            context.Services.AddSingleton<IHeaderViewModelFactory, HeaderViewModelFactory>();
            context.Services.AddSingleton<IAddressBookService, AddressBookService>();
            context.Services.AddSingleton<CartItemViewModelFactory>();
            context.Services.AddSingleton<ICartService, CartService>();
            context.Services.AddSingleton<CartViewModelFactory>();
            context.Services.AddSingleton<IOrdersService, OrdersService>();
            context.Services.AddSingleton<ShipmentViewModelFactory>();
            context.Services.AddSingleton<IShippingService, ShippingService>();
            context.Services.AddSingleton<ICampaignService, CampaignService>();
            context.Services.AddSingleton<IHtmlDownloader, HtmlDownloader>();
            context.Services.AddTransient<IMailService, MailService>();
            context.Services.AddSingleton<BlogTagFactory>();
            context.Services.AddTransient<IPaymentMethod, BudgetPaymentOption>();
            context.Services.AddTransient<IPaymentMethod, CashOnDeliveryPaymentOption>();
            context.Services.AddTransient<IPaymentMethod, GenericCreditCardPaymentOption>();
            context.Services.AddTransient<IPaymentMethod, GiftCardPaymentOption>();
            context.Services.AddSingleton<ISearchService, SearchService>();
            context.Services.AddSingleton<CatalogContentClientConventions, FoundationFindConventions>();
            context.Services.AddSingleton<CatalogContentEventListener, FoundationCatalogContentEventListener>();
            context.Services.AddSingleton<IModelBinderProvider, FilterOptionModelBinderProvider>();
            context.Services.AddSingleton<IModelBinderProvider, PaymentModelBinderProvider>();
            context.Services.AddTransient<IContentQuery, LandingPagesSlice>();
            context.Services.AddTransient<IContentSlice, LandingPagesSlice>();
            context.Services.AddTransient<IContentQuery, StandardPagesSlice>();
            context.Services.AddTransient<IContentSlice, StandardPagesSlice>();
            context.Services.AddTransient<IContentQuery, BlogsSlice>();
            context.Services.AddTransient<IContentSlice, BlogsSlice>();
            context.Services.AddTransient<IContentQuery, BlocksSlice>();
            context.Services.AddTransient<IContentSlice, BlocksSlice>();
            context.Services.AddTransient<IContentQuery, MediaSlice>();
            context.Services.AddTransient<IContentSlice, MediaSlice>();
            context.Services.AddTransient<IContentQuery, ImagesSlice>();
            context.Services.AddTransient<IContentSlice, ImagesSlice>();
            context.Services.AddTransient<IContentQuery, EverythingSlice>();
            context.Services.AddTransient<IContentSlice, EverythingSlice>();
            context.Services.AddTransient<IContentQuery, MyContentSlice>();
            context.Services.AddTransient<IContentSlice, MyContentSlice>();
            context.Services.AddTransient<IContentQuery, MyPagesSlice>();
            context.Services.AddTransient<IContentSlice, MyPagesSlice>();
            context.Services.AddTransient<IContentQuery, UnusedMediaSlice>();
            context.Services.AddTransient<IContentSlice, UnusedMediaSlice>();
            context.Services.AddTransient<IContentQuery, UnusedBlocksSlice>();
            context.Services.AddTransient<IContentSlice, UnusedBlocksSlice>();
            context.Services.AddTransient<IContentQuery, ProductsSlice>();
            context.Services.AddTransient<IContentSlice, ProductsSlice>();
            context.Services.AddTransient<IContentQuery, PackagesSlice>();
            context.Services.AddTransient<IContentSlice, PackagesSlice>();
            context.Services.AddTransient<IContentQuery, BundlesSlice>();
            context.Services.AddTransient<IContentSlice, BundlesSlice>();
            context.Services.AddTransient<IContentQuery, VariantsSlice>();
            context.Services.AddTransient<IContentSlice, VariantsSlice>();
            context.Services.AddTransient<IContentQuery, OrderPromotionsSlice>();
            context.Services.AddTransient<IContentSlice, OrderPromotionsSlice>();
            context.Services.AddTransient<IContentQuery, ShippingPromotionsSlice>();
            context.Services.AddTransient<IContentSlice, ShippingPromotionsSlice>();
            context.Services.AddTransient<IContentQuery, EntryPromotionsSlice>();
            context.Services.AddTransient<IContentSlice, EntryPromotionsSlice>();
            context.Services.AddSingleton<ISchemaDataMapper<BlogItemPage>, BlogItemPageSchemaMapper>();
            context.Services.AddSingleton<ISchemaDataMapper<HomePage>, HomePageSchemaMapper>();
            context.Services.AddSingleton<ISchemaDataMapper<GenericProduct>, GenericProductSchemaDataMapper>();
            context.Services.AddSingleton<ISchemaDataMapper<LocationItemPage>, LocationItemPageSchemaDataMapper>();
        }

        public void Initialize(InitializationEngine context)
        {
            var manager = context.Locate.Advanced.GetInstance<MigrationManager>();
            if (manager.SiteNeedsToBeMigrated())
            {
                manager.Migrate();
            }

            ViewEngines.Engines.Insert(0, new FeaturesViewEngine());
            context.InitializeFoundationCommerce();
            context.InitializeFoundationFindCms();

            var handler = GlobalConfiguration.Configuration.MessageHandlers
                .FirstOrDefault(x => x.GetType() == typeof(PassiveAuthenticationMessageHandler));

            if (handler != null)
            {
                GlobalConfiguration.Configuration.MessageHandlers.Remove(handler);
            }

            context.InitComplete += ContextOnInitComplete;

            SearchClient.Instance.Conventions.UnifiedSearchRegistry
                .ForInstanceOf<LocationListPage>()
                .ProjectImageUriFrom(page => new Uri(context.Locate.Advanced.GetInstance<UrlResolver>().GetUrl(page.PageImage), UriKind.Relative));

            SearchClient.Instance.Conventions.ForInstancesOf<LocationItemPage>().IncludeField(dp => dp.TagString());
        }

        public void Uninitialize(InitializationEngine context) => context.InitComplete -= ContextOnInitComplete;

        private void ContextOnInitComplete(object sender, EventArgs eventArgs)
        {
            _services.AddTransient<ContentAreaRenderer, FoundationContentAreaRenderer>();
            Extensions.InstallDefaultContent();
        }
    }
}