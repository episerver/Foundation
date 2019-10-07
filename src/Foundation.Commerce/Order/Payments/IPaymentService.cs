using Foundation.Commerce.Order.ViewModels;
using System.Collections.Generic;

namespace Foundation.Commerce.Order.Payments
{
    public interface IPaymentService
    {
        IEnumerable<PaymentMethodViewModel> GetPaymentMethodsByMarketIdAndLanguageCode(string marketId, string languageCode);
    }
}