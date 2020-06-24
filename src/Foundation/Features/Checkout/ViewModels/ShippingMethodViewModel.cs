using Mediachase.Commerce;
using System;

namespace Foundation.Features.Checkout.ViewModels
{
    public class ShippingMethodViewModel
    {
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public Money Price { get; set; }
        public bool IsInstorePickup { get; set; }
    }
}