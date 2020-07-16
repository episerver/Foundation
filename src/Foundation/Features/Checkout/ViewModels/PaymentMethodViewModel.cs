using EPiServer.Commerce.Order;
using System;

namespace Foundation.Features.Checkout.ViewModels
{
    public class PaymentMethodViewModel
    {
        public PaymentMethodViewModel()
        {
        }

        public PaymentMethodViewModel(IPaymentMethod paymentOption)
        {
            PaymentMethodId = paymentOption.PaymentMethodId;
            SystemKeyword = paymentOption.SystemKeyword;
            FriendlyName = paymentOption.Name;
            Description = paymentOption.Description;

            PaymentOption = paymentOption;
        }

        public Guid PaymentMethodId { get; set; }
        public string SystemKeyword { get; set; }
        public string FriendlyName { get; set; }
        public string Description { get; set; }
        public bool IsDefault { get; set; }
        public IPaymentMethod PaymentOption { get; set; }
    }
}