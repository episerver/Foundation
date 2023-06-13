using Foundation.Features.Checkout.ViewModels;
using Mediachase.Commerce.Orders;

namespace Foundation.Features.Checkout.Services
{
    public interface IShippingService
    {
        IList<ShippingMethodInfoModel> GetShippingMethodsByMarket(string marketid, bool returnInactive);
        ShippingMethodInfoModel GetInstorePickupModel();
        ShippingRate GetRate(IShipment shipment, ShippingMethodInfoModel shippingMethodInfoModel, IMarket currentMarket);
    }
}
