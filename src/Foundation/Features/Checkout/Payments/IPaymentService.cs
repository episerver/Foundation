using Foundation.Features.Checkout.ViewModels;
using System.Collections.Generic;

namespace Foundation.Features.Checkout.Payments
{
    public interface IPaymentService
    {
        IEnumerable<PaymentMethodViewModel> GetPaymentMethodsByMarketIdAndLanguageCode(string marketId, string languageCode);
    }
}