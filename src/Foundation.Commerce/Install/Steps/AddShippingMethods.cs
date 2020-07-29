using EPiServer;
using Foundation.Cms.Extensions;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Orders.Dto;
using Mediachase.Commerce.Orders.Managers;
using Mediachase.Commerce.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace Foundation.Commerce.Install.Steps
{
    public class AddShippingMethods : BaseInstallStep
    {
        public AddShippingMethods(IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            IMarketService marketService) : base(contentRepository, referenceConverter, marketService)
        {
        }

        public override int Order => 6;

        public override string Description => "Adds shipping methods to Foundation.";

        protected override void ExecuteInternal(IProgressMessenger progressMessenger)
        {
            using (var stream = new FileStream(Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"App_Data\shippingMethods.xml"), FileMode.Open))
            {
                var enabledMarkets = MarketService.GetAllMarkets().Where(x => x.IsEnabled).ToList();
                foreach (var language in enabledMarkets.SelectMany(x => x.Languages).Distinct())
                {
                    var languageId = language.TwoLetterISOLanguageName;
                    var dto = ShippingManager.GetShippingMethods(languageId);
                    DeleteShippingMethods(dto);
                    ShippingManager.SaveShipping(dto);

                    var shippingOption = dto.ShippingOption.First(x => x.Name == "Generic Gateway");
                    var shippingMethods = new List<ShippingMethodDto.ShippingMethodRow>();

                    foreach (var xShippingMethod in GetXElements(stream, "ShippingMethod"))
                    {
                        var method = new
                        {
                            Name = xShippingMethod.Get("Name"),
                            Description = xShippingMethod.Get("Description"),
                            CostInUSD = xShippingMethod.GetDecimalOrDefault("CostInUSD"),
                            SortOrder = xShippingMethod.GetIntOrDefault("SortOrder")
                        };
                        foreach (var currency in enabledMarkets.Where(x => x.Languages.Contains(language)).SelectMany(m => m.Currencies).Distinct())
                        {
                            shippingMethods.Add(CreateShippingMethod(
                                dto,
                                shippingOption,
                                languageId,
                                method.SortOrder,
                                method.Name + "-" + currency,
                                string.Format(method.Description, currency, languageId), new Money(method.CostInUSD, currency),
                                currency));
                        }
                    }
                    ShippingManager.SaveShipping(dto);

                    AssociateShippingMethodWithMarkets(dto, enabledMarkets.Where(x => x.Languages.Contains(language)), shippingMethods);
                    ShippingManager.SaveShipping(dto);
                }
            }
        }

        private void DeleteShippingMethods(ShippingMethodDto dto)
        {
            foreach (var method in dto.ShippingMethod)
            {
                method.Delete();
            }
        }

        private ShippingMethodDto.ShippingMethodRow CreateShippingMethod(ShippingMethodDto dto, ShippingMethodDto.ShippingOptionRow shippingOption, string languageId, int sortOrder, string name, string description, Money costInUsd, Currency currency)
        {
            var shippingCost = CurrencyFormatter.ConvertCurrency(costInUsd, currency);
            if (shippingCost.Currency != currency)
            {
                throw new InvalidOperationException("Cannot convert to currency " + currency + " Missing conversion data.");
            }
            return dto.ShippingMethod.AddShippingMethodRow(
                Guid.NewGuid(),
                shippingOption,
                languageId,
                true,
                name,
                "",
                shippingCost.Amount,
                shippingCost.Currency,
                description,
                false,
                sortOrder,
                DateTime.Now,
                DateTime.Now);
        }

        private void AssociateShippingMethodWithMarkets(ShippingMethodDto dto, IEnumerable<IMarket> markets, IEnumerable<ShippingMethodDto.ShippingMethodRow> shippingSet)
        {
            foreach (var shippingMethod in shippingSet)
            {
                foreach (var market in markets.Where(m => m.Currencies.Contains(shippingMethod.Currency)))
                {
                    dto.MarketShippingMethods.AddMarketShippingMethodsRow(market.MarketId.Value, shippingMethod);
                }
            }
        }
    }
}
