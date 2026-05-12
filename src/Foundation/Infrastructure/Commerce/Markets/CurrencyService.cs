using Foundation.Infrastructure.Cms;

namespace Foundation.Infrastructure.Commerce.Markets
{
    public class CurrencyService : ICurrencyService
    {
        private const string CurrencyCookie = "Currency";
        private readonly ICookieService _cookieService;
        private readonly ICurrentMarket _currentMarket;

        public CurrencyService(ICurrentMarket currentMarket, ICookieService cookieService)
        {
            _currentMarket = currentMarket;
            _cookieService = cookieService;
        }

        private IMarket CurrentMarket => _currentMarket.GetCurrentMarket();

        public IEnumerable<Currency> GetAvailableCurrencies() => CurrentMarket?.Currencies ?? Enumerable.Empty<Currency>();

        public virtual Currency GetCurrentCurrency()
        {
            // CurrentMarket is null when ICurrentMarket.GetCurrentMarket() returns null (e.g. on DXP
            // before a market is associated with the current request context). Fall back to USD so
            // Money(0, currency) in CartViewModelFactory does not throw "The currency is empty".
            if (CurrentMarket == null)
                return new Currency("USD");

            return TryGetCurrency(_cookieService.Get(CurrencyCookie), out var currency)
                ? currency
                : CurrentMarket.DefaultCurrency;
        }

        public bool SetCurrentCurrency(string currencyCode)
        {
            if (!TryGetCurrency(currencyCode, out _))
            {
                return false;
            }

            _cookieService.Set(CurrencyCookie, currencyCode);

            return true;
        }

        private bool TryGetCurrency(string currencyCode, out Currency currency)
        {
            var result = GetAvailableCurrencies()
                .Where(x => x.CurrencyCode == currencyCode)
                .Cast<Currency?>()
                .FirstOrDefault();

            if (result.HasValue)
            {
                currency = result.Value;
                return true;
            }

            currency = null;
            return false;
        }
    }
}