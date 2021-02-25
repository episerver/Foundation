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
            var discountedPrice = GetDiscountPrices(new[] { contentLink }, market, currency, _referenceConverter);
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

        private IEnumerable<DiscountedEntry> GetDiscountPrices(
          IEnumerable<ContentReference> entryLinks,
          IMarket market,
          Mediachase.Commerce.Currency marketCurrency,
          Mediachase.Commerce.Catalog.ReferenceConverter referenceConverter)
        {
            if (entryLinks is null || (market is null) || marketCurrency.IsEmpty)
            {
                throw new ArgumentNullException(nameof(marketCurrency));
            }

            List<DiscountedEntry> source = new List<DiscountedEntry>();
            HashSet<string> entryCodes = new HashSet<string>(entryLinks.Select<ContentReference, string>(new Func<ContentReference, string>(referenceConverter.GetCode)));
            Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();
            foreach (RewardDescription rewardDescription in _promotionEngine.Evaluate(entryLinks, market, marketCurrency, RequestFulfillmentStatus.Fulfilled))
            {
                HashSet<string> usedCodes = new HashSet<string>();
                foreach (ILineItem lineItem in rewardDescription.Redemptions.Where<RedemptionDescription>((Func<RedemptionDescription, bool>)(x => x.AffectedEntries != null)).SelectMany<RedemptionDescription, PriceEntry>((Func<RedemptionDescription, IEnumerable<PriceEntry>>)(x => x.AffectedEntries.PriceEntries)).Where<PriceEntry>((Func<PriceEntry, bool>)(x => x != null)).Select<PriceEntry, ILineItem>((Func<PriceEntry, ILineItem>)(x => x.ParentItem)).Where<ILineItem>((Func<ILineItem, bool>)(x => !usedCodes.Contains(x.Code))).Where<ILineItem>((Func<ILineItem, bool>)(x => entryCodes.Contains(x.Code))))
                {
                    usedCodes.Add(lineItem.Code);
                    ContentReference entryLink = referenceConverter.GetContentLink(lineItem.Code);
                    DiscountedEntry discountedEntry = source.SingleOrDefault<DiscountedEntry>((Func<DiscountedEntry, bool>)(x => x.EntryLink == entryLink));
                    if (discountedEntry == null)
                    {
                        discountedEntry = new DiscountedEntry(entryLink, (IList<DiscountPrice>)new List<DiscountPrice>());
                        source.Add(discountedEntry);
                    }
                    if (dictionary.ContainsKey(lineItem.Code))
                    {
                        dictionary[lineItem.Code] -= rewardDescription.SavedAmount;
                    }
                    else
                    {
                        // lineItemCalculator.GetExtendedPrice(lineItem, marketCurrency).Amount;
                        decimal amount = PriceCalculationService.GetSalePrice(lineItem.Code, market.MarketId, marketCurrency).UnitPrice.Amount;
                        dictionary.Add(lineItem.Code, amount - rewardDescription.SavedAmount);
                    }
                    DiscountPrice discountPrice = new DiscountPrice((EntryPromotion)rewardDescription.Promotion, new Money(Math.Max(dictionary[lineItem.Code], 0M), marketCurrency), new Money(lineItem.PlacedPrice, marketCurrency));
                    discountedEntry.DiscountPrices.Add(discountPrice);
                }
            }
            return (IEnumerable<DiscountedEntry>)source;
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