using System.Collections.Generic;

namespace Foundation.Features.Checkout.ViewModels
{
    public class UpdateShippingMethodViewModel
    {
        public IList<ShipmentViewModel> Shipments { get; set; }
    }
}