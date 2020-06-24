using EPiServer.Commerce.Catalog.ContentTypes;
using Foundation.Features.Stores;
using Mediachase.Commerce;
using System.Collections.Generic;

namespace Foundation.Features.Checkout.ViewModels
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }

        public string DisplayName { get; set; }

        public string ImageUrl { get; set; }

        public string Url { get; set; }

        public string Brand { get; set; }

        public Money? DiscountedPrice { get; set; }

        public Money PlacedPrice { get; set; }

        public string Code { get; set; }

        public EntryContentBase Entry { get; set; }

        public decimal Quantity { get; set; }

        public Money? DiscountedUnitPrice { get; set; }

        public IEnumerable<string> AvailableSizes { get; set; }

        public bool IsAvailable { get; set; }

        public bool OnSale { get; set; }

        public bool NewArrival { get; set; }

        public string AddressId { get; set; }

        public bool IsGift { get; set; }

        public string Description { get; set; }

        public string LongDescription { get; set; }

        public StoreViewModel Stores { get; set; }

        public bool IsFeaturedProduct { get; set; }

        public bool IsBestBetProduct { get; set; }
    }
}