using Foundation.Infrastructure.Commerce;
using Mediachase.Commerce.Markets;

namespace Foundation.Features.Markets
{
    public class MarketsViewComponent : ViewComponent
    {
        private readonly IMarketService _marketService;
        private readonly ICurrentMarket _currentMarket;
        private readonly UrlResolver _urlResolver;
        private readonly LanguageService _languageService;
        private readonly ICurrencyService _currencyService;
        private const string FlagLocation = "/icons/flags/";
        private const string ViewName = "~/Features/Markets/Index.cshtml";

        public MarketsViewComponent(IMarketService marketService,
            ICurrentMarket currentMarket,
            UrlResolver urlResolver,
            LanguageService languageService,
            ICurrencyService currencyService)
        {
            _marketService = marketService;
            _currentMarket = currentMarket;
            _urlResolver = urlResolver;
            _languageService = languageService;
            _currencyService = currencyService;
        }

        public IViewComponentResult Invoke(ContentReference contentLink)
        {
            var currentMarket = _currentMarket.GetCurrentMarket();

            if (CacheManager.Get(Constant.CacheKeys.MarketViewModel + "-" + currentMarket.MarketId.Value) is MarketViewModel cache)
            {
                return View(ViewName, cache);
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
                return View(ViewName, marketViewModel);
            }
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
