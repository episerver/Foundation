using EPiServer.Commerce.Order;
using EPiServer.Framework.Localization;
using Foundation.Commerce.Markets;
using Mediachase.Commerce;
using System;
using System.Linq;

namespace Foundation.Features.Checkout.Payments
{
    public abstract class PaymentOptionBase : IPaymentMethod
    {
        protected readonly LocalizationService LocalizationService;
        protected readonly IOrderGroupFactory OrderGroupFactory;

        public Guid PaymentMethodId { get; }
        public abstract string SystemKeyword { get; }
        public string Name { get; }
        public string Description { get; }
        public Money Amount { get; set; }

        protected PaymentOptionBase(LocalizationService localizationService,
            IOrderGroupFactory orderGroupFactory,
            ICurrentMarket currentMarket,
            LanguageService languageService,
            IPaymentService paymentService)
        {
            LocalizationService = localizationService;
            OrderGroupFactory = orderGroupFactory;

            if (!string.IsNullOrEmpty(SystemKeyword))
            {
                var currentMarketId = currentMarket.GetCurrentMarket().MarketId.Value;
                var currentLanguage = languageService.GetCurrentLanguage().TwoLetterISOLanguageName;
                var availablePaymentMethods = paymentService.GetPaymentMethodsByMarketIdAndLanguageCode(currentMarketId, currentLanguage);
                var paymentMethod = availablePaymentMethods.FirstOrDefault(m => m.SystemKeyword.Equals(SystemKeyword));

                if (paymentMethod != null)
                {
                    PaymentMethodId = paymentMethod.PaymentMethodId;
                    Name = paymentMethod.FriendlyName;
                    Description = paymentMethod.Description;
                }
            }
        }

        public abstract IPayment CreatePayment(decimal amount, IOrderGroup orderGroup);

        public abstract bool ValidateData();
    }
}