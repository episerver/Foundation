using Foundation.Features.Stores;
using Mediachase.Commerce;
using System;

namespace Foundation.Features.CatalogContent
{
    public class ProductTileViewModel : IProductModel
    {
        public int ProductId { get; set; }
        public string DisplayName { get; set; }
        public string VideoAssetUrl { get; set; }
        public string ImageUrl { get; set; }
        public string Url { get; set; }
        public string Brand { get; set; }
        public string Description { get; set; }
        public string LongDescription { get; set; }
        public Money? DiscountedPrice { get; set; }
        public Money PlacedPrice { get; set; }
        public string Code { get; set; }
        public bool IsAvailable { get; set; }
        public bool OnSale { get; set; }
        public bool NewArrival { get; set; }
        public StoreViewModel Stores { get; set; }
        public bool IsFeaturedProduct { get; set; }
        public bool IsBestBetProduct { get; set; }
        public bool HasBestBetStyle { get; set; }
        public bool ShowRecommendations { get; set; }
        public string FirstVariationCode { get; set; }
        public Type EntryType { get; set; }
        public string ProductStatus { get; set; }
        public DateTime Created { get; set; }
    }
}