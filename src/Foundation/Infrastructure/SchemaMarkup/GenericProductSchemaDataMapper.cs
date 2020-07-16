using Foundation.Cms;
using Foundation.Cms.Extensions;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Markets;
using Foundation.Features.CatalogContent.Product;
using Mediachase.Commerce;
using Schema.NET;
using System;
using System.Linq;

namespace Foundation.Infrastructure.SchemaMarkup
{
    /// <summary>
    /// Map GenericProduct to Schema.org product object
    /// </summary>
    public class GenericProductSchemaDataMapper : ISchemaDataMapper<GenericProduct>
    {
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyService;

        public GenericProductSchemaDataMapper(ICurrentMarket currentMarket, ICurrencyService currencyService)
        {
            _currentMarket = currentMarket;
            _currencyService = currencyService;
        }

        public Thing Map(GenericProduct content)
        {
            var variants = content.VariationContents();

            //Set availability based on inventory
            var availability = ItemAvailability.OutOfStock;
            var inventories = content.Inventories();
            if (inventories.Any(x => !x.IsTracked || x.InStockQuantity > x.ReorderMinQuantity))
            {
                availability = ItemAvailability.InStock;
            }
            else if (inventories.Any(x => x.InStockQuantity > 0))
            {
                availability = ItemAvailability.LimitedAvailability;
            }

            //Set prices or price range
            var prices = content.Prices();
            var minPrice = prices.Any() ? prices.Min(x => x.UnitPrice) : new Money();
            var maxPrice = prices.Any() ? prices.Max(x => x.UnitPrice) : new Money();
            var priceEndDate = prices.Any() ?
                prices.Where(x => x.UnitPrice.Equals(minPrice) || x.UnitPrice.Equals(maxPrice)).Min(x => x.ValidUntil ?? DateTime.MaxValue)
                : DateTime.Now;

            var offer = new Offer
            {
                PriceCurrency = minPrice.Currency.CurrencyCode,
                ItemCondition = OfferItemCondition.NewCondition,
                Availability = availability
            };

            //Handle single price vs price range
            if (minPrice.Equals(maxPrice))
            {
                offer.Price = minPrice.Amount;
                offer.PriceValidUntil = new DateTimeOffset(priceEndDate);
            }
            else
            {
                offer.PriceSpecification = new PriceSpecification
                {
                    MinPrice = minPrice.Amount,
                    MaxPrice = maxPrice.Amount,
                    ValidThrough = new DateTimeOffset(priceEndDate)
                };
            }

            return new Product
            {
                Name = content.DisplayName,
                Image = content.CommerceMediaCollection?.Select(x => x.AssetLink.GetUri(content.Language.Name, true)).ToList(),
                Description = EPiServer.Core.Html.TextIndexer.StripHtml(content.LongDescription?.ToHtmlString(), int.MaxValue),
                Sku = variants.Select(x => x.Code).ToList(),
                Brand = new Brand
                {
                    Name = content.Brand ?? string.Empty
                },
                Offers = offer
            };
        }
    }
}
