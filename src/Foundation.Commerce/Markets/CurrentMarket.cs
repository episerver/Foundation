using Foundation.Cms;
using Foundation.Commerce.Customer.Services;
using Mediachase.Commerce;
using Mediachase.Commerce.Core;
using Mediachase.Commerce.Markets;
using System;

namespace Foundation.Commerce.Markets
{
    public class CurrentMarket : ICurrentMarket
    {
        private const string MarketCookie = "MarketId";
        private static readonly MarketId DefaultMarketId = new MarketId("US");
        private readonly CookieService _cookieService;
        private readonly IMarketService _marketService;
        private readonly ICustomerService _customerService;

        public CurrentMarket(IMarketService marketService,
            CookieService cookieService,
            ICustomerService customerService)
        {
            _marketService = marketService;
            _cookieService = cookieService;
            _customerService = customerService;
        }

        public IMarket GetCurrentMarket()
        {
            var currentMarket = _cookieService.Get(MarketCookie);
            if (string.IsNullOrEmpty(currentMarket))
            {
                currentMarket = _customerService.GetCurrentContact()?.UserLocationId;
                if (!string.IsNullOrEmpty(currentMarket))
                {
                    return GetMarket(new MarketId(currentMarket));
                }

                currentMarket = DefaultMarketId.Value;
            }

            return GetMarket(new MarketId(currentMarket));
        }

        public void SetCurrentMarket(MarketId marketId)
        {
            var market = GetMarket(marketId);
            SiteContext.Current.Currency = market.DefaultCurrency;
            _cookieService.Set(MarketCookie, marketId.Value);
            MarketEvent.OnChangeMarket(market, new EventArgs());
        }

        private IMarket GetMarket(MarketId marketId) => _marketService.GetMarket(marketId) ?? _marketService.GetMarket(DefaultMarketId);
    }
}