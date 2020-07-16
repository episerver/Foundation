using EPiServer.Security;
using EPiServer.ServiceLocation;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Checkout
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

            var filter = new PriceFilter
            {
                CustomerPricing = customerPricing,
                Currencies = new List<Currency> { currency },
                Quantity = 1m,
                ReturnCustomerPricing = true
            };

            var prices = _priceService.Service.GetPrices(marketId, DateTime.Now, new CatalogKey(entryCode), filter);

            if (prices.Any())
            {
                return prices.OrderBy(x => x.UnitPrice.Amount).First();
            }

            return null;
        }
    }
}