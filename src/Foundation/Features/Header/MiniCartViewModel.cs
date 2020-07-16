using EPiServer.Core;
using Foundation.Features.Checkout.ViewModels;
using Mediachase.Commerce;
using System.Collections.Generic;

namespace Foundation.Features.Header
{
    public class MiniCartViewModel
    {
        public MiniCartViewModel()
        {
            Shipments = new List<ShipmentViewModel>();
        }

        public ContentReference CheckoutPage { get; set; }

        public ContentReference CartPage { get; set; }

        public decimal ItemCount { get; set; }

        public IEnumerable<ShipmentViewModel> Shipments { get; set; }

        public Money Total { get; set; }

        public string Label { get; set; }

        public bool IsSharedCart { get; set; }
    }

    public class MiniWishlistViewModel
    {
        public MiniWishlistViewModel()
        {
            Items = new List<CartItemViewModel>();
        }

        public ContentReference WishlistPage { get; set; }

        public decimal ItemCount { get; set; }

        public IEnumerable<CartItemViewModel> Items { get; set; }

        public Money Total { get; set; }

        public string Label { get; set; }

        public bool HasOrganization { get; set; }
    }
}