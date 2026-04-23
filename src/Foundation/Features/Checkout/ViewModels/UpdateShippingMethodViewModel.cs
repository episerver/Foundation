namespace Foundation.Features.Checkout.ViewModels
{
    public class UpdateShippingMethodViewModel
    {
        public IList<ShipmentViewModel> Shipments { get; set; }

        public string SystemKeyword { get; set; }

        public Guid PaymentMethodId { get; set; }
    }
}