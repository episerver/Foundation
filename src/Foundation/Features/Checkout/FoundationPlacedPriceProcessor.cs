using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Order;
using Foundation.Commerce;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Pricing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Checkout
{
    public class FoundationPlacedPriceProcessor : DefaultPlacedPriceProcessor
    {
        private readonly IContentLoader _contentLoader;
        private readonly MapUserKey _mapUserKey;
        private readonly IPriceService _priceService;
        private readonly ReferenceConverter _referenceConverter;

        public FoundationPlacedPriceProcessor(
            IPriceService priceService,
            IContentLoader contentLoader,
            ReferenceConverter referenceConverter,
            MapUserKey mapUserKey,
            ReferenceConverter referenceConverter1,
            MapUserKey mapUserKey1)
            : base(priceService, contentLoader, referenceConverter, mapUserKey)
        {
            _priceService = priceService;
            _contentLoader = contentLoader;
            _referenceConverter = referenceConverter1;
            _mapUserKey = mapUserKey1;
        }

        public override bool UpdatePlacedPrice(ILineItem lineItem, CustomerContact customerContact, MarketId marketId,
            Currency currency, Action<ILineItem, ValidationIssue> onValidationError)
        {
            var entryContent = lineItem.GetEntryContent(_referenceConverter, _contentLoader);
            if (entryContent == null)
            {
                onValidationError(lineItem, ValidationIssue.RemovedDueToUnavailableItem);
                return false;
            }

            if (lineItem.Properties[Constant.Quote.PreQuotePrice] != null &&
                !string.IsNullOrEmpty(lineItem.Properties[Constant.Quote.PreQuotePrice].ToString()))
            {
                return true;
            }

            var placedPrice = GetPlacedPrice(entryContent, lineItem.Quantity, customerContact, marketId, currency);
            if (placedPrice.HasValue)
            {
                if (new Money(currency.Round(lineItem.PlacedPrice), currency) == placedPrice.Value)
                {
                    return true;
                }

                onValidationError(lineItem, ValidationIssue.PlacedPricedChanged);
                lineItem.PlacedPrice = placedPrice.Value.Amount;
                return true;
            }

            onValidationError(lineItem, ValidationIssue.RemovedDueToInvalidPrice);
            return false;
        }

        public override Money? GetPlacedPrice(
            EntryContentBase entry,
            decimal quantity,
            CustomerContact customerContact,
            MarketId marketId,
            Currency currency)
        {
            var customerPricing = new List<CustomerPricing>
            {
                CustomerPricing.AllCustomers
            };

            if (customerContact != null)
            {
                var userKey = _mapUserKey.ToUserKey(customerContact.UserId);
                if (userKey != null && !string.IsNullOrWhiteSpace(userKey.ToString()))
                {
                    customerPricing.Add(new CustomerPricing(CustomerPricing.PriceType.UserName, userKey.ToString()));
                }

                if (!string.IsNullOrEmpty(customerContact.EffectiveCustomerGroup))
                {
                    customerPricing.Add(new CustomerPricing(CustomerPricing.PriceType.PriceGroup,
                        customerContact.EffectiveCustomerGroup));
                }
            }

            var priceFilter = new PriceFilter
            {
                Currencies = new List<Currency> { currency },
                Quantity = quantity,
                CustomerPricing = customerPricing,
                ReturnCustomerPricing = false
            };

            var priceValue = _priceService
                .GetPrices(marketId, DateTime.UtcNow, new CatalogKey(entry.Code), priceFilter)
                .OrderBy(pv => pv.UnitPrice)
                .FirstOrDefault();

            return priceValue?.UnitPrice;
        }
    }
}