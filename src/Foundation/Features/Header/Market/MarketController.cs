using EPiServer.Core;
using EPiServer.Web.Routing;
using Foundation.Cms.Extensions;
using Foundation.Commerce.Markets;
using Foundation.Features.Checkout.Services;
using Mediachase.Commerce;
using Mediachase.Commerce.Markets;
using System.Web.Mvc;

namespace Foundation.Features.Header.Market
{
    public class MarketController : Controller
    {
        private readonly IMarketService _marketService;
        private readonly ICurrentMarket _currentMarket;
        private readonly UrlResolver _urlResolver;
        private readonly LanguageService _languageService;
        private readonly ICartService _cartService;
        private readonly ICurrencyService _currencyService;

        public MarketController(
            IMarketService marketService,
            ICurrentMarket currentMarket,
            UrlResolver urlResolver,
            LanguageService languageService,
            ICartService cartService,
            ICurrencyService currencyService)
        {
            _marketService = marketService;
            _currentMarket = currentMarket;
            _urlResolver = urlResolver;
            _languageService = languageService;
            _cartService = cartService;
            _currencyService = currencyService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Set(string marketId, ContentReference contentLink)
        {
            var newMarketId = new MarketId(marketId);
            _currentMarket.SetCurrentMarket(newMarketId);
            var currentMarket = _marketService.GetMarket(newMarketId);
            var cart = _cartService.LoadCart(_cartService.DefaultCartName, true)?.Cart;

            if (cart != null && cart.Currency != null)
            {
                _currencyService.SetCurrentCurrency(cart.Currency);
            }
            else
            {
                _currencyService.SetCurrentCurrency(currentMarket.DefaultCurrency);
            }

            _languageService.UpdateLanguage(currentMarket.DefaultLanguage.Name);

            var returnUrl = _urlResolver.GetUrl(Request, contentLink, currentMarket.DefaultLanguage.Name);
            return Json(new { returnUrl });
        }
    }
}