using EPiServer;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Catalog.Dto;
using Mediachase.Commerce.Catalog.Managers;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Install.Steps
{
    public class AddCurrencies : BaseInstallStep
    {
        public AddCurrencies(IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            IMarketService marketService) : base(contentRepository, referenceConverter, marketService)
        {
        }

        public override int Order => 2;

        public override string Description => "Adds currency conversions to Foundation.";

        protected override void ExecuteInternal(IProgressMessenger progressMessenger) => new CurrencySetup().CreateConversions();
    }

    public class CurrencySetup
    {
        private class CurrencyConversion
        {
            public CurrencyConversion(string currency, string name, decimal factor)
            {
                Currency = currency;
                Name = name;
                Factor = factor;
            }

            public string Currency;
            public string Name;
            public decimal Factor;
        }

        private readonly CurrencyConversion[] _conversionRatesToUsd = new[] {
            new CurrencyConversion("USD", "US dollar", 1m),
            new CurrencyConversion("SEK", "Swedish krona", 0.12m),
            new CurrencyConversion("AUD", "Australian dollar", 0.78m),
            new CurrencyConversion("CAD", "Canadian dollar", 0.81m),
            new CurrencyConversion("EUR", "Euro", 1.07m),
            new CurrencyConversion("BRL", "Brazilian Real", 0.33m),
            new CurrencyConversion("CLP", "Chilean Peso", 0.001637m),
            new CurrencyConversion("JPY", "Japanese yen", 0.008397m),
            new CurrencyConversion("NOK", "Norwegian krone", 0.128333m),
            new CurrencyConversion("SAR", "Saudi Arabian Riyal", 0.734m),
            new CurrencyConversion("GBP", "Pound sterling", 1.49m) };

        public void CreateConversions()
        {
            EnsureCurrencies();

            var dto = CurrencyManager.GetCurrencyDto();
            foreach (var conversion in _conversionRatesToUsd)
            {
                var toCurrencies = _conversionRatesToUsd.Where(c => c != conversion).ToList();
                AddRates(dto, conversion, toCurrencies);
            }
            CurrencyManager.SaveCurrency(dto);
        }

        private void EnsureCurrencies()
        {
            var isDirty = false;
            var dto = CurrencyManager.GetCurrencyDto();
            foreach (var conversion in _conversionRatesToUsd)
            {
                if (GetCurrency(dto, conversion.Currency) == null)
                {
                    dto.Currency.AddCurrencyRow(conversion.Currency, conversion.Name, DateTime.Now);
                    isDirty = true;
                }
            }

            if (isDirty)
            {
                CurrencyManager.SaveCurrency(dto);
            }
        }

        private void AddRates(CurrencyDto dto, CurrencyConversion from, IEnumerable<CurrencyConversion> toCurrencies)
        {
            var rates = dto.CurrencyRate;
            foreach (var to in toCurrencies)
            {
                var rate = (double)(from.Factor / to.Factor);
                var fromRow = GetCurrency(dto, from.Currency);
                var toRow = GetCurrency(dto, to.Currency);
                rates.AddCurrencyRateRow(rate, rate, DateTime.Now, fromRow, toRow, DateTime.Now);
            }
        }

        private CurrencyDto.CurrencyRow GetCurrency(CurrencyDto dto, string currencyCode) => (CurrencyDto.CurrencyRow)dto.Currency.Select("CurrencyCode = '" + currencyCode + "'").SingleOrDefault();
    }
}
