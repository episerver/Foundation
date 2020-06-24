using Foundation.Commerce.Markets;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.CatalogContent.Services
{
    public interface IPricingService
    {
        IList<IPriceValue> GetPriceList(string code, MarketId marketId, PriceFilter priceFilter);
        IList<IPriceValue> GetPriceList(IEnumerable<CatalogKey> catalogKeys, MarketId marketId, PriceFilter priceFilter);
        Money? GetCurrentPrice(string code);
        Money? GetPrice(string code, MarketId marketId, Currency currency);
    }

    public class PricingService : IPricingService
    {
        private readonly IPriceService _priceService;
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyService;

        public PricingService(IPriceService priceService,
            ICurrentMarket currentMarket,
            ICurrencyService currencyService)
        {
            _priceService = priceService;
            _currentMarket = currentMarket;
            _currencyService = currencyService;
        }

        public IList<IPriceValue> GetPriceList(string code, MarketId marketId, PriceFilter priceFilter)
        {
            if (string.IsNullOrEmpty(code))
            {
                throw new ArgumentNullException(nameof(code));
            }

            var catalogKey = new CatalogKey(code);

            return _priceService.GetPrices(marketId, DateTime.Now, catalogKey, priceFilter)
                .OrderBy(x => x.UnitPrice.Amount)
                .ToList();
        }

        public IList<IPriceValue> GetPriceList(IEnumerable<CatalogKey> catalogKeys, MarketId marketId, PriceFilter priceFilter)
        {
            if (catalogKeys == null)
            {
                throw new ArgumentNullException(nameof(catalogKeys));
            }

            if (!catalogKeys.Any())
            {
                return Enumerable.Empty<IPriceValue>().ToList();
            }

            return _priceService.GetPrices(marketId, DateTime.Now, catalogKeys, priceFilter)
                .OrderBy(x => x.UnitPrice.Amount)
                .ToList();
        }

        public Money? GetCurrentPrice(string code)
        {
            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyService.GetCurrentCurrency();
            return GetPrice(code, market.MarketId, currency);
        }

        public Money? GetPrice(string code, MarketId marketId, Currency currency)
        {
            var prices = GetPriceList(code, marketId,
                new PriceFilter
                {
                    Currencies = new[] { currency }
                });

            return prices.Any() ? prices.First().UnitPrice : (Money?)null;
        }
    }
}