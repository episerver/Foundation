using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Models.Pages;
using System.Collections.Generic;

namespace Foundation.Commerce.Order.ViewModels
{
    public class UpdateAddressViewModel
    {
        public AddressModel BillingAddress { get; set; }
        public CheckoutPage CurrentPage { get; set; }
        public int ShippingAddressIndex { get; set; }
        public bool UseBillingAddressForShipment { get; set; }
        public IList<ShipmentViewModel> Shipments { get; set; }
    }
}