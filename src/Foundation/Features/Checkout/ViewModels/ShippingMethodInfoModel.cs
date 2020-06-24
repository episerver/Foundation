using System;

namespace Foundation.Features.Checkout.ViewModels
{
    public class ShippingMethodInfoModel
    {
        public Guid MethodId { get; set; }
        public string ClassName { get; set; }
        public string LanguageId { get; set; }
        public string Currency { get; set; }
        public int Ordering { get; set; }
        public string Name { get; set; }
    }
}