using Foundation.Cms;
using Mediachase.Commerce;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Markets
{
    public class CurrencyService : ICurrencyService
    {
        private const string CurrencyCookie = "Currency";
        private readonly CookieService _cookieService;
        private readonly ICurrentMarket _currentMarket;

        public CurrencyService(ICurrentMarket currentMarket, CookieService cookieService)
        {
            _currentMarket = currentMarket;
            _cookieService = cookieService;
        }

        private IMarket CurrentMarket => _currentMarket.GetCurrentMarket();

        public IEnumerable<Currency> GetAvailableCurrencies() => CurrentMarket.Currencies;

        public virtual Currency GetCurrentCurrency()
        {
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