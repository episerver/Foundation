using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Globalization;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Infrastructure.Cms;
using Foundation.Infrastructure.Commerce.Customer.Services;
using Foundation.Infrastructure.Commerce.Install;
using Foundation.Infrastructure.Commerce.Install.Steps;
using Foundation.Infrastructure.Commerce.Marketing;
using Microsoft.Extensions.DependencyInjection;

namespace Foundation.Infrastructure.Commerce
{
    [ModuleDependency(typeof(Cms.Initialize))]
    public class Initialize : IConfigurableModule
    {
        void IConfigurableModule.ConfigureContainer(ServiceConfigurationContext context)
        {
            var _services = context.Services;
            _services.AddSingleton<ICurrentMarket, CurrentMarket>();
            _services.AddSingleton<IAddressBookService, AddressBookService>();
            _services.AddSingleton<ICustomerService, CustomerService>();
            _services.AddSingleton<IFileHelperService, FileHelperService>();
            _services.AddTransient<ILoyaltyService, LoyaltyService>();
            _services.AddSingleton<IUniqueCouponService, UniqueCouponService>();
            _services.AddSingleton<ICurrencyService, CurrencyService>();
            _services.AddSingleton<MarketContentLoader>();
            _services.AddSingleton<ICouponFilter, FoundationCouponFilter>();
            _services.AddSingleton<ICouponUsage, FoundationCouponUsage>();
            _services.AddSingleton<IInstallService, InstallService>();
            _services.AddSingleton<IInstallStep, AddCurrencies>();
            _services.AddSingleton<IInstallStep, AddCustomers>();
            _services.AddSingleton<IInstallStep, AddMarkets>();
            _services.AddSingleton<IInstallStep, AddPaymentMethods>();
            _services.AddSingleton<IInstallStep, AddPromotions>();
            _services.AddSingleton<IInstallStep, AddShippingMethods>();
            _services.AddSingleton<IInstallStep, AddTaxes>();
            _services.AddSingleton<IInstallStep, AddWarehouses>();
            context.ConfigurationComplete += (o, e) =>
            {
                e.Services.Intercept<IUpdateCurrentLanguage>(
                (locator, defaultImplementation) =>
                    new LanguageService(
                        locator.GetInstance<ICurrentMarket>(),
                        locator.GetInstance<ICookieService>(),
                        defaultImplementation));
            };
        }

        void IInitializableModule.Initialize(InitializationEngine context)
        {
            //GlobalFilters.Filters.Add(new AJAXLocalizationFilterAttribute());

            //var contentOptions = context.Locate.Advanced.GetInstance<ContentManagerOptions>();
            //contentOptions.EnsureCommerceLoaded();

            var associationDefinitionRepository = context.Locate.Advanced.GetInstance<GroupDefinitionRepository<AssociationGroupDefinition>>();
            associationDefinitionRepository.Add(new AssociationGroupDefinition { Name = "Accessory" });
            associationDefinitionRepository.Add(new AssociationGroupDefinition { Name = "Part" });
            associationDefinitionRepository.Add(new AssociationGroupDefinition { Name = "Related product" });
            associationDefinitionRepository.Add(new AssociationGroupDefinition { Name = "Cross sell" });
            associationDefinitionRepository.Add(new AssociationGroupDefinition { Name = "Up sell" });
        }

        void IInitializableModule.Uninitialize(InitializationEngine context)
        {
        }
    }
}