using EPiServer.Core;
using System.Collections.Generic;

namespace Foundation.Features.Markets
{
    public class MarketViewModel
    {
        public MarketItem CurrentMarket { get; set; }
        public string CurrentLanguage { get; set; }
        public string CurrentCurrency { get; set; }
        public IEnumerable<MarketItem> Markets { get; set; }
        public IEnumerable<LanguageItem> Languages { get; set; }
        public IEnumerable<CurrencyItem> Currencies { get; set; }
        public ContentReference ContentLink { get; set; }
    }

    public class MarketItem
    {
        public string Value { get; set; }
        public string DisplayName { get; set; }
        public string FlagUrl { get; set; }
    }

    public class LanguageItem
    {
        public string Value { get; set; }
        public string DisplayName { get; set; }
    }

    public class CurrencyItem
    {
        public string Value { get; set; }
        public string DisplayName { get; set; }
        public string Symbol { get; set; }
    }
}