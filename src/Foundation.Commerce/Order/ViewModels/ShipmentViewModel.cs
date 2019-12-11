using Foundation.Commerce.Customer.ViewModels;
using Mediachase.Commerce;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Order.ViewModels
{
    public class ShipmentViewModel
    {
        public int ShipmentId { get; set; }

        public IList<CartItemViewModel> CartItems { get; set; }

        public AddressModel Address { get; set; }

        public Guid ShippingMethodId { get; set; }

        public IEnumerable<ShippingMethodViewModel> ShippingMethods { get; set; }

        public string CurrentShippingMethodName { get; set; }

        public Money CurrentShippingMethodPrice { get; set; }

        public Money GetShipmentItemsTotal(Currency currency) => CartItems.Aggregate(new Money(0, currency), (current, item) => current + item.DiscountedPrice.GetValueOrDefault());

        public int ShippingAddressType { get; set; }
        public string WarehouseCode { get; set; }
    }
}