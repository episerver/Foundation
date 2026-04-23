namespace Foundation.Features.Checkout.ViewModels
{
    public class UpdateAddressViewModel
    {
        public string AddressId { get; set; }

        public bool UseBillingAddressForShipment { get; set; }

        /// <summary>
        /// To determine the shipment index when editing shipping address
        /// </summary>
        public int ShippingAddressIndex { get; set; }

        /// <summary>
        /// AddressType can be Shipping or Billing
        /// </summary>
        public string AddressType { get; set; }
    }

    public static class AddressType
    {
        public const string Billing = "Billing";
        public const string Shipping = "Shipping";
    }
}