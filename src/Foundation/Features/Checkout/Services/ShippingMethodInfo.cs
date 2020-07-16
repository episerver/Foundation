using System;

namespace Foundation.Features.Checkout.Services
{
    public class ShippingMethodInfo
    {
        public Guid MethodId { get; set; }

        public string ClassName { get; set; }

        public string LanguageId { get; set; }

        public string Currency { get; set; }

        public int Ordering { get; set; }
    }
}