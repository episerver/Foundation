using EPiServer.Commerce.Order;
using Foundation.Features.Checkout.ViewModels;
using Mediachase.Commerce;
using Mediachase.Commerce.Orders;
using System.Collections.Generic;

namespace Foundation.Features.Checkout.Services
{
    public interface IShippingService
    {
        IList<ShippingMethodInfoModel> GetShippingMethodsByMarket(string marketid, bool returnInactive);
        ShippingMethodInfoModel GetInstorePickupModel();
        ShippingRate GetRate(IShipment shipment, ShippingMethodInfoModel shippingMethodInfoModel, IMarket currentMarket);
    }
}
