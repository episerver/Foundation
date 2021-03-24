using EPiBootstrapArea;
using EPiBootstrapArea.Initialization;
using EPiServer;
using EPiServer.Commerce.Internal.Migration;
using EPiServer.Commerce.Marketing.Internal;
using EPiServer.Commerce.Order;
using EPiServer.ContentApi.Core.Configuration;
using EPiServer.ContentApi.Search;
using EPiServer.Core;
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
using Foundation.Cms.Settings;
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
using Foundation.Features.Settings;
using Foundation.Features.Shared;
using Foundation.Features.Stores;
using Foundation.Find.Facets;
using Foundation.Find.Facets.Config;
using Foundation.Infrastructure.Display;
using Foundation.Infrastructure.PowerSlices;
using Foundation.Infrastructure.SchemaMarkup;
using Foundation.Infrastructure.Services;
using Mediachase.Commerce.Orders;
using Mediachase.MetaDataPlus.Configurator;
using PowerSlice;
using System;
using System.Collections.Generic;
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
        private IServiceLocator _locator;

        public void ConfigureContainer(ServiceConfigurationContext context)
        {
            _services = context.Services;
            context.ConfigureFoundationCms();

            _services.Configure<ContentApiConfiguration>(c =>
            {
                c.EnablePreviewFeatures = true;
                c.Default().SetMinimumRoles(string.Empty).SetRequiredRole(string.Empty);
            });

            _services.Configure<ContentApiSearchConfiguration>(config =>
            {
                config.Default()
                .SetMaximumSearchResults(200)
                .SetSearchCacheDuration(TimeSpan.FromMinutes(60));
            });

            _services.AddSingleton<IDisplayModeFallbackProvider, FoundationDisplayModeProvider>();
            _services.AddTransient<IQuickNavigatorItemProvider, FoundationQuickNavigatorItemProvider>();
            _services.AddTransient<IViewTemplateModelRegistrator, ViewTemplateModelRegistrator>();
            _services.AddSingleton<DefaultPlacedPriceProcessor, FoundationPlacedPriceProcessor>();
            _services.AddSingleton<ISearchViewModelFactory, SearchViewModelFactory>();
            _services.AddSingleton<IPaymentService, PaymentService>();
            _services.AddTransient<CheckoutViewModelFactory>();
            _services.AddSingleton<MultiShipmentViewModelFactory>();
            _services.AddSingleton<OrderSummaryViewModelFactory>();
            _services.AddTransient<PaymentMethodViewModelFactory>();
            _services.AddSingleton<IBookmarksService, BookmarksService>();
            _services.AddSingleton<IPricingService, PricingService>();
            _services.AddSingleton<IB2BNavigationService, B2BNavigationService>();
            _services.AddSingleton<IBudgetService, BudgetService>();
            _services.AddSingleton<ICreditCardService, CreditCardService>();
            _services.AddSingleton<IGiftCardService, GiftCardService>();
            _services.AddSingleton<IOrganizationService, OrganizationService>();
            _services.AddSingleton<IQuickOrderService, QuickOrderService>();
            _services.AddSingleton<IProductService, ProductService>();
            _services.AddSingleton<IPromotionService, PromotionService>();
            _services.AddSingleton<IStoreService, StoreService>();
            _services.AddSingleton<CatalogEntryViewModelFactory>();
            _services.AddSingleton<IHeaderViewModelFactory, HeaderViewModelFactory>();
            _services.AddSingleton<IAddressBookService, AddressBookService>();
            _services.AddSingleton<CartItemViewModelFactory>();
            _services.AddSingleton<ICartService, CartService>();
            _services.AddSingleton<CartViewModelFactory>();
            _services.AddSingleton<IOrdersService, OrdersService>();
            _services.AddSingleton<ShipmentViewModelFactory>();
            _services.AddSingleton<IShippingService, ShippingService>();
            _services.AddSingleton<ICampaignService, CampaignService>();
            _services.AddSingleton<IHtmlDownloader, HtmlDownloader>();
            _services.AddTransient<IMailService, MailService>();
            _services.AddSingleton<BlogTagFactory>();
            _services.AddTransient<IPaymentMethod, BudgetPaymentOption>();
            _services.AddTransient<IPaymentMethod, CashOnDeliveryPaymentOption>();
            _services.AddTransient<IPaymentMethod, GenericCreditCardPaymentOption>();
            _services.AddTransient<IPaymentMethod, GiftCardPaymentOption>();
            _services.AddSingleton<ISearchService, SearchService>();
            _services.AddSingleton<CatalogContentClientConventions, FoundationFindConventions>();
            _services.AddSingleton<CatalogContentEventListener, FoundationCatalogContentEventListener>();
            _services.AddSingleton<IModelBinderProvider, FilterOptionModelBinderProvider>();
            _services.AddSingleton<IModelBinderProvider, PaymentModelBinderProvider>();
            _services.AddTransient<IContentQuery, LandingPagesSlice>();
            _services.AddTransient<IContentSlice, LandingPagesSlice>();
            _services.AddTransient<IContentQuery, StandardPagesSlice>();
            _services.AddTransient<IContentSlice, StandardPagesSlice>();
            _services.AddTransient<IContentQuery, BlogsSlice>();
            _services.AddTransient<IContentSlice, BlogsSlice>();
            _services.AddTransient<IContentQuery, BlocksSlice>();
            _services.AddTransient<IContentSlice, BlocksSlice>();
            _services.AddTransient<IContentQuery, MediaSlice>();
            _services.AddTransient<IContentSlice, MediaSlice>();
            _services.AddTransient<IContentQuery, ImagesSlice>();
            _services.AddTransient<IContentSlice, ImagesSlice>();
            _services.AddTransient<IContentQuery, EverythingSlice>();
            _services.AddTransient<IContentSlice, EverythingSlice>();
            _services.AddTransient<IContentQuery, MyContentSlice>();
            _services.AddTransient<IContentSlice, MyContentSlice>();
            _services.AddTransient<IContentQuery, MyPagesSlice>();
            _services.AddTransient<IContentSlice, MyPagesSlice>();
            _services.AddTransient<IContentQuery, UnusedMediaSlice>();
            _services.AddTransient<IContentSlice, UnusedMediaSlice>();
            _services.AddTransient<IContentQuery, UnusedBlocksSlice>();
            _services.AddTransient<IContentSlice, UnusedBlocksSlice>();
            _services.AddTransient<IContentQuery, ProductsSlice>();
            _services.AddTransient<IContentSlice, ProductsSlice>();
            _services.AddTransient<IContentQuery, PackagesSlice>();
            _services.AddTransient<IContentSlice, PackagesSlice>();
            _services.AddTransient<IContentQuery, BundlesSlice>();
            _services.AddTransient<IContentSlice, BundlesSlice>();
            _services.AddTransient<IContentQuery, VariantsSlice>();
            _services.AddTransient<IContentSlice, VariantsSlice>();
            _services.AddTransient<IContentQuery, OrderPromotionsSlice>();
            _services.AddTransient<IContentSlice, OrderPromotionsSlice>();
            _services.AddTransient<IContentQuery, ShippingPromotionsSlice>();
            _services.AddTransient<IContentSlice, ShippingPromotionsSlice>();
            _services.AddTransient<IContentQuery, EntryPromotionsSlice>();
            _services.AddTransient<IContentSlice, EntryPromotionsSlice>();
            _services.AddSingleton<ISchemaDataMapper<BlogItemPage>, BlogItemPageSchemaMapper>();
            _services.AddSingleton<ISchemaDataMapper<HomePage>, HomePageSchemaMapper>();
            _services.AddSingleton<ISchemaDataMapper<GenericProduct>, GenericProductSchemaDataMapper>();
            _services.AddSingleton<ISchemaDataMapper<LocationItemPage>, LocationItemPageSchemaDataMapper>();
            _services.AddSingleton<PromotionEngineContentLoader, FoundationPromotionEngineContentLoader>();
        }

        public void Initialize(InitializationEngine context)
        {
            _locator = context.Locate.Advanced;
            var manager = context.Locate.Advanced.GetInstance<MigrationManager>();
            if (manager.SiteNeedsToBeMigrated())
            {
                manager.Migrate();
            }

            ViewEngines.Engines.Insert(0, new FeaturesViewEngine());
            context.InitializeFoundationCommerce();

            var handler = GlobalConfiguration.Configuration.MessageHandlers
                .FirstOrDefault(x => x.GetType() == typeof(PassiveAuthenticationMessageHandler));

            if (handler != null)
            {
                GlobalConfiguration.Configuration.MessageHandlers.Remove(handler);
            }

            context.InitComplete += ContextOnInitComplete;
            context.InitComplete += AddMetaFieldLineItem;

            SearchClient.Instance.Conventions.UnifiedSearchRegistry
                .ForInstanceOf<LocationListPage>()
                .ProjectImageUriFrom(page => new Uri(context.Locate.Advanced.GetInstance<UrlResolver>().GetUrl(page.PageImage), UriKind.Relative));

            SearchClient.Instance.Conventions.ForInstancesOf<LocationItemPage>().IncludeField(dp => dp.TagString());
        }

        public void Uninitialize(InitializationEngine context)
        {
            context.InitComplete -= ContextOnInitComplete;
            context.InitComplete -= AddMetaFieldLineItem;
            context.Locate.Advanced.GetInstance<IContentEvents>().PublishedContent -= OnPublishedContent;
        }

        private void ContextOnInitComplete(object sender, EventArgs eventArgs)
        {
            _services.AddTransient<ContentAreaRenderer, FoundationContentAreaRenderer>();
            Extensions.InstallDefaultContent();
            var settings = _locator.GetInstance<ISettingsService>().GetSiteSettings<SearchSettings>();
            if (settings != null)
            {
                InitializeFacets(settings.SearchFiltersConfiguration);
            }

            _locator.GetInstance<IContentEvents>().PublishedContent += OnPublishedContent;
        }

        private void OnPublishedContent(object sender, ContentEventArgs contentEventArgs)
        {
            if (contentEventArgs.Content is IFacetConfiguration facetConfiguration)
            {
                InitializeFacets(facetConfiguration.SearchFiltersConfiguration);
            }
        }

        private void InitializeFacets(IList<FacetFilterConfigurationItem> configItems)
        {
            if (configItems != null && configItems.Any())
            {
                _locator.GetInstance<IFacetRegistry>().Clear();
                configItems
                    .ToList()
                    .ForEach(x => _locator.GetInstance<IFacetRegistry>().AddFacetDefinitions(_locator.GetInstance<IFacetConfigFactory>().GetFacetDefinition(x)));
            }
        }

        private void AddMetaFieldLineItem(object sender, EventArgs eventArgs)
        {
            var lineItemMetaClass = OrderContext.Current.LineItemMetaClass;
            var context = OrderContext.MetaDataContext;

            var name = "VariantOptionCodes";
            var displayName = "Variant Option Codes";
            var length = 256;
            var metaFieldType = MetaDataType.LongString;
            var metaNamespace = string.Empty;
            var description = string.Empty;
            var isNullable = false;
            var isMultiLanguage = true;
            var isSearchable = true;
            var isEncrypted = true;

            var metaField = MetaField.Load(context, name) ?? MetaField.Create(context,
                                             lineItemMetaClass.Namespace,
                                             name,
                                             displayName,
                                             description,
                                             metaFieldType,
                                             length,
                                             isNullable,
                                             isMultiLanguage,
                                             isSearchable,
                                             isEncrypted);

            if (lineItemMetaClass.MetaFields.All(x => x.Id != metaField.Id))
            {
                lineItemMetaClass.AddField(metaField);
            }
        }
    }
}