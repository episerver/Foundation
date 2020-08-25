using EPiServer.Commerce.Order;
using Foundation.Commerce.GiftCard;
using Foundation.Commerce.Markets;
using Foundation.Features.Checkout.Payments;
using Mediachase.Commerce;
using Mediachase.Commerce.Customers;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Checkout.ViewModels
{
    public class PaymentMethodViewModelFactory
    {
        private readonly ICurrentMarket _currentMarket;
        private readonly LanguageService _languageService;
        private readonly IPaymentService _paymentService;
        private readonly IEnumerable<IPaymentMethod> _paymentOptions;
        private readonly IGiftCardService _giftCardService;

        public PaymentMethodViewModelFactory(
            ICurrentMarket currentMarket,
            LanguageService languageService,
            IPaymentService paymentService,
            IEnumerable<IPaymentMethod> paymentOptions,
            IGiftCardService giftCardService)
        {
            _currentMarket = currentMarket;
            _languageService = languageService;
            _paymentService = paymentService;
            _paymentOptions = paymentOptions;
            _giftCardService = giftCardService;
        }

        public IEnumerable<PaymentMethodViewModel> GetPaymentMethodViewModels()
        {
            var currentMarket = _currentMarket.GetCurrentMarket().MarketId;
            var currentLanguage = _languageService.GetCurrentLanguage().TwoLetterISOLanguageName;
            var availablePaymentMethods = _paymentService.GetPaymentMethodsByMarketIdAndLanguageCode(currentMarket.Value, currentLanguage);
            var availableCustomerGiftCards = _giftCardService.GetCustomerGiftCards(CustomerContext.Current.CurrentContactId.ToString()).Where(g => g.IsActive == true);

            var displayedPaymentMethods = availablePaymentMethods
                .Where(p => _paymentOptions.Any(m => m.PaymentMethodId == p.PaymentMethodId))
                .Select(p => new PaymentMethodViewModel(_paymentOptions.First(m => m.PaymentMethodId == p.PaymentMethodId)) { IsDefault = p.IsDefault })
                .ToList();

            if (availableCustomerGiftCards.Any() == false)
            {
                displayedPaymentMethods.RemoveAll(x => x.SystemKeyword == "GiftCardPayment");
            }

            return displayedPaymentMethods;
        }
    }
}
