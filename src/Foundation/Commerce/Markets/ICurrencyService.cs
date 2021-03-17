using Mediachase.Commerce;
using System.Collections.Generic;

namespace Foundation.Commerce.Markets
{
    public interface ICurrencyService
    {
        IEnumerable<Currency> GetAvailableCurrencies();
        Currency GetCurrentCurrency();
        bool SetCurrentCurrency(string currencyCode);
    }
}