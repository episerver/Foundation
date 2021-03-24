using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Order;
using EPiServer.Tracking.Commerce.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;

namespace Foundation.Personalization
{
    public interface ICommerceTrackingService
    {
        Task<TrackingResponseData> TrackProduct(HttpContextBase httpContext, string productCode,
            bool skipRecommendations);

        Task<TrackingResponseData> TrackSearch(HttpContextBase httpContext, string searchTerm, int pageSize,
            IEnumerable<string> productCodes);

        Task<TrackingResponseData> TrackOrder(HttpContextBase httpContext, IPurchaseOrder order);
        Task<TrackingResponseData> TrackCategory(HttpContextBase httpContext, NodeContent category);
        Task<TrackingResponseData> TrackCart(HttpContextBase httpContext, ICart cart);
        Task<TrackingResponseData> TrackWishlist(HttpContextBase httpContext);
        Task<TrackingResponseData> TrackCheckout(HttpContextBase httpContext);
        Task<TrackingResponseData> TrackHome(HttpContextBase httpContext);
        Task<TrackingResponseData> TrackBrand(HttpContextBase httpContext, string brandName);

        Task<TrackingResponseData> TrackAttribute(HttpContextBase httpContext, string attributeName,
            string attributeValue);

        Task<TrackingResponseData> TrackDefault(HttpContextBase httpContext);
    }
}