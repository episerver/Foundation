using EPiServer;
using Foundation.Cms.Extensions;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Shared;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace Foundation.Commerce.Install.Steps
{
    public class AddMarkets : BaseInstallStep
    {
        public AddMarkets(IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            IMarketService marketService) : base(contentRepository, referenceConverter, marketService)
        {
        }

        public override int Order => 1;

        public override string Description => "Adds markets to Foundation.";

        protected override void ExecuteInternal(IProgressMessenger progressMessenger)
        {
            progressMessenger.AddProgressMessageText("Creating markets...", false, 0);
            using (var stream = new FileStream(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"App_Data\markets.xml"), FileMode.Open))
            {
                foreach (var xMarket in GetXElements(stream, "Market"))
                {
                    var market = new MarketImpl(xMarket.Get("MarketId"))
                    {
                        IsEnabled = xMarket.GetBool("IsEnabled"),
                        MarketName = xMarket.Get("MarketName"),
                        MarketDescription = xMarket.Get("MarketDescription") ?? xMarket.Get("MarketName"),
                        DefaultCurrency = new Currency(xMarket.Get("DefaultCurrency")),
                        DefaultLanguage = new CultureInfo(xMarket.Get("DefaultLanguage")),
                        PricesIncludeTax = xMarket.GetBoolOrDefault("PricesIncludeTax")
                    };

                    foreach (var xCurrency in xMarket.Element("Currencies").Elements("Currency").Distinct())
                    {
                        market.CurrenciesCollection.Add(new Currency((string)xCurrency));
                    }

                    foreach (var xLanguage in xMarket.Element("Languages").Elements("Language").Distinct())
                    {
                        market.LanguagesCollection.Add(new CultureInfo((string)xLanguage));
                    }

                    foreach (var xCountry in xMarket.Element("Countries").Elements("Country").Distinct())
                    {
                        market.CountriesCollection.Add((string)xCountry);
                    }

                    var existingMarket = MarketService.GetMarket(market.MarketId);
                    if (existingMarket == null)
                    {
                        MarketService.CreateMarket(market);
                    }
                    else
                    {
                        foreach (var currency in existingMarket.Currencies.Where(x => !market.CurrenciesCollection.Contains(x)))
                        {
                            market.CurrenciesCollection.Add(currency);
                        }

                        foreach (var language in existingMarket.Languages
                            .Where(el => !market.Languages.Any(nl => string.Equals(el.Name, nl.Name, StringComparison.OrdinalIgnoreCase))))
                        {
                            market.LanguagesCollection.Add(language);
                        }

                        foreach (var country in existingMarket.Countries
                            .Where(ec => !market.Countries.Any(nc => string.Equals(ec, nc, StringComparison.OrdinalIgnoreCase))))
                        {
                            market.CountriesCollection.Add(country);
                        }

                        MarketService.UpdateMarket(market);
                    }
                }
            }
        }
    }
}
