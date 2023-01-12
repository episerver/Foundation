using EPiServer.Tracking.Commerce.Data;

namespace Foundation.Infrastructure.Personalization
{
    public interface ICommerceTrackingService
    {
        Task<TrackingResponseData> TrackProduct(HttpContext httpContext, string productCode,
            bool skipRecommendations);

        Task<TrackingResponseData> TrackSearch(HttpContext httpContext, string searchTerm, int pageSize,
            IEnumerable<string> productCodes);

        Task<TrackingResponseData> TrackOrder(HttpContext httpContext, IPurchaseOrder order);
        Task<TrackingResponseData> TrackCategory(HttpContext httpContext, NodeContent category);
        Task<TrackingResponseData> TrackCart(HttpContext httpContext, ICart cart);
        Task<TrackingResponseData> TrackWishlist(HttpContext httpContext);
        Task<TrackingResponseData> TrackCheckout(HttpContext httpContext);
        Task<TrackingResponseData> TrackHome(HttpContext httpContext);
        Task<TrackingResponseData> TrackBrand(HttpContext httpContext, string brandName);

        Task<TrackingResponseData> TrackAttribute(HttpContext httpContext, string attributeName,
            string attributeValue);

        Task<TrackingResponseData> TrackDefault(HttpContext httpContext);
    }
}