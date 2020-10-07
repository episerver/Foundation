using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Routing;
using Foundation.Cms.Extensions;
using Foundation.Commerce;
using Foundation.Commerce.Markets;
using Foundation.Features.Checkout.Services;
using Mediachase.Commerce;
using Mediachase.Commerce.Markets;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Markets
{
    public class MarketController : Controller
    {
        private readonly IMarketService _marketService;
        private readonly ICurrentMarket _currentMarket;
        private readonly UrlResolver _urlResolver;
        private readonly LanguageService _languageService;
        private readonly ICartService _cartService;
        private readonly ICurrencyService _currencyService;
        private const string FlagLocation = "/Assets/icons/flags/";

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

        [ChildActionOnly]
        public ActionResult Index(ContentReference contentLink)
        {
            var currentMarket = _currentMarket.GetCurrentMarket();

            if (CacheManager.Get(Constant.CacheKeys.MarketViewModel + "-" + currentMarket.MarketId.Value) is MarketViewModel cache)
            {
                return PartialView(cache);
            }
            else
            {
                var markets = _marketService.GetAllMarkets().Where(x => x.IsEnabled).OrderBy(x => x.MarketName)
                    .Select(x => new MarketItem
                    {
                        DisplayName = x.MarketName,
                        Value = x.MarketId.Value,
                        FlagUrl = GetFlagUrl(x.MarketId.Value)
                    });
                var languages = _languageService.GetAvailableLanguages()
                    .Select(x => new LanguageItem
                    {
                        DisplayName = x.NativeName,
                        Value = x.Name
                    });
                var currencies = _currencyService.GetAvailableCurrencies()
                    .Select(x => new CurrencyItem
                    {
                        DisplayName = x.CurrencyCode,
                        Value = x.CurrencyCode,
                        Symbol = x.Format.CurrencySymbol
                    });
                var marketViewModel = new MarketViewModel
                {
                    CurrentMarket = new MarketItem
                    {
                        Value = currentMarket.MarketId.Value,
                        DisplayName = currentMarket.MarketName,
                        FlagUrl = GetFlagUrl(currentMarket.MarketId.Value)
                    },
                    CurrentLanguage = _languageService.GetCurrentLanguage().Name,
                    CurrentCurrency = _currencyService.GetCurrentCurrency().Format.CurrencySymbol,
                    Markets = markets,
                    Languages = languages,
                    Currencies = currencies,
                    ContentLink = contentLink,
                };

                return PartialView(marketViewModel);
            }
        }

        [HttpPost]
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

        protected virtual string GetFlagUrl(string marketId)
        {
            switch (marketId)
            {
                case "AUS":
                    return $"{FlagLocation}australia.svg";
                case "BRA":
                    return $"{FlagLocation}brazil.svg";
                case "CAN":
                    return $"{FlagLocation}canada.svg";
                case "CHL":
                    return $"{FlagLocation}chile.svg";
                case "DEU":
                    return $"{FlagLocation}germany.svg";
                case "ESP":
                    return $"{FlagLocation}spain.svg";
                case "FR":
                    return $"{FlagLocation}france.svg";
                case "JPN":
                    return $"{FlagLocation}japan.svg";
                case "NLD":
                    return $"{FlagLocation}netherlands.svg";
                case "NOR":
                    return $"{FlagLocation}norway.svg";
                case "SAU":
                    return $"{FlagLocation}saudi-arabia.svg";
                case "SWE":
                    return $"{FlagLocation}sweden.svg";
                case "UK":
                    return $"{FlagLocation}united-kingdom.svg";
                case "US":
                default:
                    return $"{FlagLocation}united-states-of-america.svg";
            }
        }
    }
}