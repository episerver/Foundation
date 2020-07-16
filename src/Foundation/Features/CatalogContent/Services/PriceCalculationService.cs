using EPiServer.Security;
using EPiServer.ServiceLocation;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.CatalogContent.Services
{
    public static class PriceCalculationService
    {
        private static Injected<IPriceService> _priceService;

        public static IPriceValue GetSalePrice(string entryCode, MarketId marketId, Currency currency)
        {
            var customerPricing = new List<CustomerPricing>
            {
                new CustomerPricing(CustomerPricing.PriceType.AllCustomers, string.Empty),
                new CustomerPricing(CustomerPricing.PriceType.UserName, PrincipalInfo.Current.Name)
            };
            if (CustomerContext.Current.CurrentContact != null)
            {
                customerPricing.Add(new CustomerPricing(CustomerPricing.PriceType.PriceGroup,
                                    CustomerContext.Current.CurrentContact.EffectiveCustomerGroup));
            }

            var filter = new PriceFilter()
            {
                CustomerPricing = customerPricing,
                Currencies = new List<Currency> { currency },
                ReturnCustomerPricing = true
            };

            var prices = _priceService.Service.GetPrices(marketId, DateTime.Now, new CatalogKey(entryCode), filter);

            if (prices.Any())
            {
                return prices.OrderBy(x => x.UnitPrice.Amount).First();
            }
            else
            {
                // if the entry has no price without sale code
                prices = _priceService.Service.GetCatalogEntryPrices(new CatalogKey(entryCode))
                    .Where(x => x.ValidFrom <= DateTime.Now && (!x.ValidUntil.HasValue || x.ValidUntil.Value >= DateTime.Now))
                    .Where(x => x.UnitPrice.Currency == currency && x.MarketId == marketId);

                if (prices.Any())
                {
                    return prices.OrderBy(x => x.UnitPrice.Amount).First();
                }
                else
                {
                    return null;
                }
            }
        }

        public static IPriceValue GetSubscriptionPrice(string entryCode, MarketId marketId, Currency currency)
        {
            var filter = new PriceFilter()
            {
                CustomerPricing = new List<CustomerPricing>
                {
                    new CustomerPricing((CustomerPricing.PriceType)5, string.Empty),
                },
                Currencies = new List<Currency> { currency },
                ReturnCustomerPricing = true
            };

            var prices = _priceService.Service.GetPrices(marketId, DateTime.Now, new CatalogKey(entryCode), filter);

            if (prices.Any())
            {
                return prices.OrderBy(x => x.UnitPrice.Amount).First();
            }
            else
            {
                return null;
            }
        }

        public static IPriceValue GetMsrpPrice(string entryCode, MarketId marketId, Currency currency)
        {
            var filter = new PriceFilter()
            {
                CustomerPricing = new List<CustomerPricing>
                {
                    new CustomerPricing((CustomerPricing.PriceType)4, string.Empty),
                },
                Currencies = new List<Currency> { currency },
                ReturnCustomerPricing = true
            };

            var prices = _priceService.Service.GetPrices(marketId, DateTime.Now,
                                                new CatalogKey(entryCode), filter);

            if (prices.Any())
            {
                return prices.OrderBy(x => x.UnitPrice.Amount).First();
            }
            else
            {
                return null;
            }
        }

        public static IPriceValue GetMapPrice(string entryCode, MarketId marketId, Currency currency)
        {
            var filter = new PriceFilter()
            {
                CustomerPricing = new List<CustomerPricing>
                {
                    new CustomerPricing((CustomerPricing.PriceType)3, string.Empty),
                },
                Currencies = new List<Currency> { currency },
                ReturnCustomerPricing = true
            };

            var prices = _priceService.Service.GetPrices(marketId, DateTime.Now, new CatalogKey(entryCode), filter);

            if (prices.Any())
            {
                return prices.OrderBy(x => x.UnitPrice.Amount).First();
            }
            else
            {
                return null;
            }
        }
    }
}
