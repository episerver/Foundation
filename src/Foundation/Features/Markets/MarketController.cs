using Foundation.Features.Checkout.Services;
using Mediachase.Commerce.Markets;
using Newtonsoft.Json;

namespace Foundation.Features.Markets
{
    [ApiController]
    [Route("[controller]")]
    public class MarketController : ControllerBase
    {
        private readonly IMarketService _marketService;
        private readonly ICurrentMarket _currentMarket;
        private readonly UrlResolver _urlResolver;
        private readonly LanguageService _languageService;
        private readonly ICartService _cartService;
        private readonly ICurrencyService _currencyService;
        private readonly IContentRouteHelper _contentRouteHelper;
        private const string FlagLocation = "/icons/flags/";

        public MarketController(
            IMarketService marketService,
            ICurrentMarket currentMarket,
            UrlResolver urlResolver,
            LanguageService languageService,
            ICartService cartService,
            ICurrencyService currencyService,
            IContentRouteHelper contentRouteHelper
            )
        {
            _marketService = marketService;
            _currentMarket = currentMarket;
            _urlResolver = urlResolver;
            _languageService = languageService;
            _cartService = cartService;
            _currencyService = currencyService;
            _contentRouteHelper = contentRouteHelper;
        }

        [HttpPost]
        [Route("Set")]
        public ActionResult Set(ContentReference contentLink, [FromForm] string marketId)
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

            _languageService.SetRoutedContent(_contentRouteHelper.Content, currentMarket.DefaultLanguage.Name);

            var returnUrl = _urlResolver.GetUrl(Request, contentLink, currentMarket.DefaultLanguage.Name);
            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(new { returnUrl }),
                ContentType = "application/json",
            };
        }

    }
}