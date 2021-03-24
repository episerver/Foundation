using Mediachase.Commerce;
using Mediachase.Commerce.Pricing;
using System;

namespace Foundation.Commerce.Extensions
{
    /// <summary>
    /// Represents a price definition in a catalog entry.
    /// </summary>
    public class Price : ICloneable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Price" /> class.
        /// </summary>
        public Price()
        {
            // Parameterless constructor is needed for deserialization
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Price" /> class.
        /// </summary>
        /// <param name="entry">tThe entry content base.</param>
        /// <param name="priceValue">The price value.</param>
        public Price(IPriceValue priceValue)
        {
            CatalogEntryCode = priceValue.CatalogKey.CatalogEntryCode;

            CustomerPricing =
                (priceValue.CustomerPricing != null)
                    ? new CustomerPricing(priceValue.CustomerPricing.PriceTypeId,
                        priceValue.CustomerPricing.PriceCode)
                    : null;

            MarketId = priceValue.MarketId;
            MinQuantity = priceValue.MinQuantity;
            UnitPrice = priceValue.UnitPrice;
            ValidFrom = priceValue.ValidFrom
                .ToLocalTime(); // make sure the time has been converted from UTC to local, to avoid mismatch between Commerce manager and Catalog Mode
            ValidUntil = priceValue.ValidUntil.HasValue
                ? priceValue.ValidUntil.Value.ToLocalTime()
                : priceValue
                    .ValidUntil; // make sure the time has been converted from UTC to local, to avoid mismatch between Commerce manager and Catalog Mode
        }

        public string CatalogEntryCode { get; set; }

        /// <summary>
        /// Gets or sets the customer pricing.
        /// </summary>
        public CustomerPricing CustomerPricing { get; set; }

        /// <summary>
        /// Gets or sets the market id.
        /// </summary>
        public MarketId MarketId { get; set; }

        /// <summary>
        /// Gets or sets the minimum quantity.
        /// </summary>
        public decimal MinQuantity { get; set; }

        /// <summary>
        /// Gets or sets the unit price.
        /// </summary>
        public Money UnitPrice { get; set; }

        /// <summary>
        /// Gets or sets the valid from date.
        /// </summary>
        public DateTime ValidFrom { get; set; }

        /// <summary>
        /// Gets or sets the valid until date.
        /// </summary>
        public DateTime? ValidUntil { get; set; }

        #region ICloneable Members

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        /// <returns>
        /// A new object that is a copy of this instance.
        /// </returns>
        public object Clone()
        {
            return new Price
            {
                CustomerPricing =
                    CustomerPricing != null
                        ? new CustomerPricing(CustomerPricing.PriceTypeId, CustomerPricing.PriceCode)
                        : null,
                MarketId = MarketId,
                MinQuantity = MinQuantity,
                UnitPrice = UnitPrice,
                ValidFrom = ValidFrom,
                ValidUntil = ValidUntil
            };
        }

        #endregion

        /// <summary>
        /// Converts to a <see cref="IPriceValue"/> instance.
        /// </summary>
        /// <returns>The converted object.</returns>
        public IPriceValue ToPriceValue()
        {
            return
                new PriceValue
                {
                    CustomerPricing =
                        (CustomerPricing != null)
                            ? new CustomerPricing(CustomerPricing.PriceTypeId, CustomerPricing.PriceCode)
                            : null,
                    MarketId = MarketId,
                    MinQuantity = MinQuantity,
                    UnitPrice = UnitPrice,
                    ValidFrom = ValidFrom.ToUniversalTime(), // IPriceValue accepts time in UTC only
                    ValidUntil =
                        ValidUntil.HasValue
                            ? ValidUntil.Value.ToUniversalTime()
                            : ValidUntil // IPriceValue accepts time in UTC only
                };
        }
    }
}