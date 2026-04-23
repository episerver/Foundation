using Foundation.Features.Checkout.ViewModels;

namespace Foundation.Features.Checkout.Payments
{
    public interface IPaymentService
    {
        IEnumerable<PaymentMethodViewModel> GetPaymentMethodsByMarketIdAndLanguageCode(string marketId, string languageCode);
    }
}