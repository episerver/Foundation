using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Order;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Labs.ContentManager;
using EPiServer.ServiceLocation;
using EPiServer.Web.Mvc;
using Foundation.Cms.SchemaMarkup;
using Foundation.Cms.ViewModels.Header;
using Foundation.Commerce.Catalog;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Mail;
using Foundation.Commerce.Marketing;
using Foundation.Commerce.Markets;
using Foundation.Commerce.Models.Catalog;
using Foundation.Commerce.Order;
using Foundation.Commerce.Order.Payments;
using Foundation.Commerce.Order.Services;
using Foundation.Commerce.Order.ViewModelFactories;
using Foundation.Commerce.SchemaDataMappers;
using Foundation.Commerce.ViewModels.Header;
using Mediachase.Commerce;
using System.Web.Mvc;

namespace Foundation.Commerce
{
    [ModuleDependency(typeof(Cms.Initialize))]
    public class Initialize : IConfigurableModule
    {
        void IConfigurableModule.ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddSingleton<ICurrentMarket, CurrentMarket>();
            services.AddSingleton<IBookmarksService, BookmarksService>();
            services.AddSingleton<IHeaderViewModelFactory, CommerceHeaderViewModelFactory>();
            services.AddSingleton<IModelBinderProvider, ModelBinderProvider>();
            services.AddSingleton<IPricingService, PricingService>();
            services.AddSingleton<IProductService, ProductService>();
            services.AddSingleton<IPromotionService, PromotionService>();
            services.AddSingleton<IStoreService, StoreService>();
            services.AddSingleton<CatalogEntryViewModelFactory>();
            services.AddSingleton<IAddressBookService, AddressBookService>();
            services.AddSingleton<IB2BNavigationService, B2BNavigationService>();
            services.AddSingleton<IBudgetService, BudgetService>();
            services.AddSingleton<ICreditCardService, CreditCardService>();
            services.AddSingleton<ICustomerService, CustomerService>();
            services.AddSingleton<IFileHelperService, FileHelperService>();
            services.AddSingleton<IGiftCardService, GiftCardService>();
            services.AddTransient<ILoyaltyService, LoyaltyService>();
            services.AddSingleton<IOrganizationService, OrganizationService>();
            services.AddSingleton<IQuickOrderService, QuickOrderService>();
            services.AddSingleton<IHtmlDownloader, HtmlDownloader>();
            services.AddTransient<IMailService, MailService>();
            services.AddSingleton<UniqueCouponService>();
            services.AddSingleton<ICurrencyService, CurrencyService>();
            services.AddSingleton<MarketContentLoader>();
            services.AddSingleton<DefaultPlacedPriceProcessor, FoundationPlacedPriceProcessor>();
            services.AddSingleton<IPaymentService, PaymentService>();
            services.AddSingleton<CartItemViewModelFactory>();
            services.AddSingleton<ICartService, CartService>();
            services.AddSingleton<CartViewModelFactory>();
            services.AddSingleton<IOrdersService, OrdersService>();
            services.AddSingleton<ShipmentViewModelFactory>();
            services.AddSingleton<IShippingService, ShippingService>();
            services.AddTransient<CheckoutViewModelFactory>();
            services.AddSingleton<MultiShipmentViewModelFactory>();
            services.AddSingleton<OrderSummaryViewModelFactory>();
            services.AddTransient<PaymentMethodViewModelFactory>();
            services.AddTransient<IViewTemplateModelRegistrator, ViewTemplateModelRegistrator>();
            services.AddTransient<IPaymentMethod, BudgetPaymentOption>();
            services.AddTransient<IPaymentMethod, CashOnDeliveryPaymentOption>();
            services.AddTransient<IPaymentMethod, GenericCreditCardPaymentOption>();
            services.AddTransient<IPaymentMethod, GiftCardPaymentOption>();
            services.AddSingleton<ICouponFilter, FoundationCouponFilter>();
            services.AddSingleton<ICouponUsage, FoundationCouponUsage>();
            services.AddSingleton<ISchemaDataMapper<GenericProduct>, GenericProductSchemaDataMapper>();
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {
            var contentOptions = context.Locate.Advanced.GetInstance<ContentManagerOptions>();
            contentOptions.EnsureCommerceLoaded();
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }
    }
}