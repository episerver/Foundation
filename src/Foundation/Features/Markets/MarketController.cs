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
                        Selected = false,
                        Text = x.MarketName,
                        Value = x.MarketId.Value,
                        FlagUrl = GetFlagUrl(x.MarketId)
                    });
                var marketViewModel = new MarketViewModel
                {
                    Markets = markets,
                    MarketId = currentMarket.MarketId.Value,
                    CurrentMarket = new MarketItem
                    {
                        Selected = false,
                        Text = currentMarket.MarketName,
                        Value = currentMarket.MarketId.Value,
                        FlagUrl = GetFlagUrl(currentMarket.MarketId)
                    },
                    ContentLink = contentLink,
                };

                return PartialView();
            }
        }

        [ChildActionOnly]
        public ActionResult CurrentMarket()
        {
            return PartialView("", new CurrentMarketViewModel
            {
                CurrentMarket = _currentMarket.GetCurrentMarket().MarketName,
                CurrentLanguage = _languageService.GetCurrentLanguage().DisplayName,
                CurrentCurrency = _currencyService.GetCurrentCurrency()
            });
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
                case "AUS":

                    break;
                case "AUS":

                    break;
                case "AUS":

                    break;
                case "AUS":

                    break;
                case "AUS":

                    break;
                case "AUS":

                    break;
                case "AUS":

                    break;
                case "AUS":

                    break;
                default:
                    break;
            }

            if (marketId == new MarketId("BRA"))
            {
                
            }

            if (marketId == new MarketId("CAN"))
            {
              
            }

            if (marketId == new MarketId("CHL"))
            {
                return $"{FlagLocation}chile.svg";
            }

            if (marketId == new MarketId("FR"))
            {
                return $"{FlagLocation}france.svg";
            }

            if (marketId == new MarketId("DEU"))
            {
                return $"{FlagLocation}germany.svg";
            }
            if (marketId == new MarketId("JPN"))
            {
                return $"{FlagLocation}japan.svg";
            }


            if (marketId == new MarketId("NLD"))
            {
                return $"{FlagLocation}netherlands.svg";
            }

            if (marketId == new MarketId("NOR"))
            {
                return $"{FlagLocation}norway.svg";
            }

            if (marketId == new MarketId("SAU"))
            {
                return $"{FlagLocation}saudi-arabia.svg";
            }

            if (marketId == new MarketId("ESP"))
            {
                return $"{FlagLocation}spain.svg";
            }



            if (marketId == new MarketId("SWE"))
            {
                return $"{FlagLocation}sweden.svg";
            }



            if (marketId == new MarketId("UK"))
            {
                return $"{FlagLocation}united-kingdom.svg";
            }

            if (marketId == new MarketId("DEFAULT"))
            {
                return $"{FlagLocation}united-states-of-america.svg";
            }

            return marketId == new MarketId("US") ? $"{FlagLocation}us.svg" : "";
        }
    }
}