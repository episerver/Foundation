using Mediachase.Commerce;

namespace Foundation.Features.CatalogContent
{
    public class VariantViewModel
    {
        public string ImageUrl { get; set; }
        public Money? DiscountedPrice { get; set; }
        public Money ListingPrice { get; set; }
        public string Size { get; set; }
        public string Sku { get; set; }
        public int Quantity { get; set; }
        public decimal StockQuantity { get; set; }

        public Money YourPrice
        {
            get
            {
                if (DiscountedPrice.HasValue && DiscountedPrice.Value < ListingPrice)
                {
                    return DiscountedPrice.Value;
                }

                return ListingPrice;
            }
        }

        public Money SavePrice
        {
            get
            {
                if (DiscountedPrice.HasValue && DiscountedPrice.Value < ListingPrice)
                {
                    return ListingPrice - DiscountedPrice.Value;
                }

                return new Money(0, ListingPrice.Currency);
            }
        }
    }
}