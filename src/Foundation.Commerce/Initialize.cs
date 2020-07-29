using EPiServer.Commerce.Marketing;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Labs.ContentManager;
using EPiServer.ServiceLocation;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Install;
using Foundation.Commerce.Install.Steps;
using Foundation.Commerce.Marketing;
using Foundation.Commerce.Markets;
using Mediachase.Commerce;

namespace Foundation.Commerce
{
    [ModuleDependency(typeof(Cms.Initialize))]
    public class Initialize : IConfigurableModule
    {
        void IConfigurableModule.ConfigureContainer(ServiceConfigurationContext context)
        {
            var services = context.Services;
            services.AddSingleton<ICurrentMarket, CurrentMarket>();
            services.AddSingleton<ICustomerService, CustomerService>();
            services.AddSingleton<IFileHelperService, FileHelperService>();
            services.AddTransient<ILoyaltyService, LoyaltyService>();
            services.AddSingleton<UniqueCouponService>();
            services.AddSingleton<ICurrencyService, CurrencyService>();
            services.AddSingleton<MarketContentLoader>();
            services.AddSingleton<ICouponFilter, FoundationCouponFilter>();
            services.AddSingleton<ICouponUsage, FoundationCouponUsage>();
            services.AddSingleton<IInstallService, InstallService>();
            services.AddSingleton<IInstallStep, AddCurrencies>();
            services.AddSingleton<IInstallStep, AddCustomers>();
            services.AddSingleton<IInstallStep, AddMarkets>();
            services.AddSingleton<IInstallStep, AddPaymentMethods>();
            services.AddSingleton<IInstallStep, AddPromotions>();
            services.AddSingleton<IInstallStep, AddShippingMethods>();
            services.AddSingleton<IInstallStep, AddTaxes>();
            services.AddSingleton<IInstallStep, AddWarehouses>();
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