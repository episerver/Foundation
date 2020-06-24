using EPiServer.Commerce.Marketing;
using EPiServer.Commerce.Order;
using EPiServer.Commerce.SpecializedProperties;
using EPiServer.Core;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.CatalogContent.Services
{
    public interface IPromotionService
    {
        IList<IPriceValue> GetDiscountPriceList(IEnumerable<CatalogKey> catalogKeys, MarketId marketId, Currency currency);
        IPriceValue GetDiscountPrice(CatalogKey catalogKey, MarketId marketId, Currency currency);
        IPriceValue GetDiscountPrice(IPriceValue price, ContentReference contentLink, Currency currency, IMarket market);
    }

    public class PromotionService : IPromotionService
    {
        private readonly IMarketService _marketService;
        private readonly ReferenceConverter _referenceConverter;
        private readonly ILineItemCalculator _lineItemCalculator;
        private readonly IPromotionEngine _promotionEngine;

        public PromotionService(
            IMarketService marketService,
            ReferenceConverter referenceConverter,
            ILineItemCalculator lineItemCalculator,
            IPromotionEngine promotionEngine)
        {
            _marketService = marketService;
            _referenceConverter = referenceConverter;
            _lineItemCalculator = lineItemCalculator;
            _promotionEngine = promotionEngine;
        }

        public IList<IPriceValue> GetDiscountPriceList(IEnumerable<CatalogKey> catalogKeys, MarketId marketId, Currency currency)
        {
            var market = _marketService.GetMarket(marketId);
            if (market == null)
            {
                throw new ArgumentException(string.Format("market '{0}' does not exist", marketId));
            }

            var prices = catalogKeys.Select(x => PriceCalculationService.GetSalePrice(x.CatalogEntryCode, marketId, currency)).Where(x => x != null);
            return GetDiscountPrices(prices.ToList(), market, currency);
        }

        public IPriceValue GetDiscountPrice(CatalogKey catalogKey, MarketId marketId, Currency currency) => GetDiscountPriceList(new[] { catalogKey }, marketId, currency).FirstOrDefault();

        public IPriceValue GetDiscountPrice(IPriceValue price, ContentReference contentLink, Currency currency, IMarket market)
        {
            var discountedPrice = _promotionEngine.GetDiscountPrices(new[] { contentLink }, market, currency, _referenceConverter, _lineItemCalculator);
            if (discountedPrice.Any())
            {
                var highestDiscount = discountedPrice.SelectMany(x => x.DiscountPrices).OrderBy(x => x.Price).FirstOrDefault().Price;
                return new PriceValue
                {
                    CatalogKey = price.CatalogKey,
                    CustomerPricing = CustomerPricing.AllCustomers,
                    MarketId = price.MarketId,
                    MinQuantity = price.MinQuantity,
                    UnitPrice = highestDiscount,
                    ValidFrom = DateTime.UtcNow,
                    ValidUntil = null
                };
            }

            return price;
        }

        public IPriceValue GetDiscountPrice(Price price, ContentReference contentLink, Currency currency, IMarket market)
        {
            var discountedPrice = _promotionEngine.GetDiscountPrices(new[] { contentLink }, market, currency, _referenceConverter, _lineItemCalculator);
            if (discountedPrice.Any())
            {
                var highestDiscount = discountedPrice.SelectMany(x => x.DiscountPrices).OrderBy(x => x.Price).FirstOrDefault().Price;
                return new PriceValue
                {
                    CatalogKey = new CatalogKey(_referenceConverter.GetCode(contentLink)),
                    CustomerPricing = CustomerPricing.AllCustomers,
                    MarketId = price.MarketId,
                    MinQuantity = price.MinQuantity,
                    UnitPrice = highestDiscount,
                    ValidFrom = DateTime.UtcNow,
                    ValidUntil = null
                };
            }

            return new PriceValue
            {
                CatalogKey = new CatalogKey(_referenceConverter.GetCode(contentLink)),
                CustomerPricing = CustomerPricing.AllCustomers,
                MarketId = price.MarketId,
                MinQuantity = price.MinQuantity,
                UnitPrice = price.UnitPrice,
                ValidFrom = DateTime.UtcNow,
                ValidUntil = null
            };
        }

        private IList<IPriceValue> GetDiscountPrices(IList<IPriceValue> prices, IMarket market, Currency currency)
        {
            currency = GetCurrency(currency, market);
            var priceValues = new List<IPriceValue>();

            foreach (var entry in GetEntries(prices))
            {
                var price = prices
                    .OrderBy(x => x.UnitPrice.Amount)
                    .FirstOrDefault(x => x.CatalogKey.CatalogEntryCode.Equals(entry.Key) &&
                        x.UnitPrice.Currency.Equals(currency));
                if (price == null)
                {
                    continue;
                }

                priceValues.Add(GetDiscountPrice(
                    price, entry.Value, currency, market));
            }

            return priceValues;
        }

        private Currency GetCurrency(Currency currency, IMarket market) => currency == Currency.Empty ? market.DefaultCurrency : currency;

        private IDictionary<string, ContentReference> GetEntries(IEnumerable<IPriceValue> prices)
        {
            return _referenceConverter.GetContentLinks(prices.GroupBy(x => x.CatalogKey.CatalogEntryCode)
                .Select(x => x.First().CatalogKey.CatalogEntryCode));
        }
    }
}