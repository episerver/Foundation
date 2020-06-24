using EPiServer.Core;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.NamedCarts.DefaultCart;
using Foundation.Features.Shared;
using Mediachase.Commerce;
using System.Collections.Generic;

namespace Foundation.Features.Header
{
    public class LargeCartViewModel : ContentViewModel<CartPage>
    {
        public LargeCartViewModel()
        {
        }

        public LargeCartViewModel(CartPage cartPage) : base(cartPage)
        {
        }

        public string ReferrerUrl { get; set; }

        public IEnumerable<ShipmentViewModel> Shipments { get; set; }

        public Money TotalDiscount { get; set; }

        public Money Total { get; set; }

        public Money Subtotal { get; set; }

        public Money ShippingTotal { get; set; }

        public Money TaxTotal { get; set; }

        public ContentReference CheckoutPage { get; set; }

        public ContentReference MultiShipmentPage { get; set; }

        public AddressModel AddressModel { get; set; }

        public IEnumerable<string> AppliedCouponCodes { get; set; }

        //public IEnumerable<Recommendation> Recommendations { get; set; }

        public bool HasOrganization { get; set; }

        public string Message { get; set; }

        public bool ShowRecommendations { get; set; }
    }
}