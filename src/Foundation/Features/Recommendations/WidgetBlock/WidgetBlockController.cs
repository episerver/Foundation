using EPiServer.Personalization.Commerce.Tracking;
using EPiServer.Tracking.Commerce.Data;
using Foundation.Features.CatalogContent.Services;
using Foundation.Features.Checkout.Services;
using Foundation.Infrastructure.Personalization;
using Newtonsoft.Json;

namespace Foundation.Features.Recommendations.WidgetBlock
{
    [ApiController]
    [Route("[controller]")]
    public class WidgetBlockController : ControllerBase
    {
        private readonly ICommerceTrackingService _trackingService;
        private readonly ReferenceConverter _referenceConverter;
        private readonly IRequiredClientResourceList _requiredClientResource;
        private readonly ICartService _cartService;
        private readonly ConfirmationService _confirmationService;
        private readonly IProductService _productService;
        private readonly IContextModeResolver _contextModeResolver;


        public WidgetBlockController(ICommerceTrackingService commerceTrackingService,
            ReferenceConverter referenceConverter,
            IRequiredClientResourceList requiredClientResource,
            ICartService cartService,
            ConfirmationService confirmationService,
            IProductService productService,
            IContextModeResolver contextModeResolver)
        {
            _trackingService = commerceTrackingService;
            _referenceConverter = referenceConverter;
            _requiredClientResource = requiredClientResource;
            _cartService = cartService;
            _confirmationService = confirmationService;
            _productService = productService;
            _contextModeResolver = contextModeResolver;
        }

        [HttpPost]
        [Route("GetRecommendations")]
        public async Task<ContentResult> GetRecommendations(string widgetType, string name, string value = "", int numberOfRecs = 4)
        {
            List<Recommendation> recommendations = null;
            TrackingResponseData response;
            switch (widgetType)
            {
                case "Home":
                    response = await _trackingService.TrackHome(ControllerContext.HttpContext);
                    recommendations = response.GetRecommendations(_referenceConverter, RecommendationsExtensions.Home)
                        .ToList();
                    break;
                case "Basket":
                    response = await _trackingService.TrackCart(ControllerContext.HttpContext, _cartService.LoadCart(_cartService.DefaultCartName, false).Cart);
                    recommendations = response.GetRecommendations(_referenceConverter, RecommendationsExtensions.Basket)
                        .ToList();
                    break;
                case "Checkout":
                    response = await _trackingService.TrackCheckout(ControllerContext.HttpContext);
                    recommendations = response.GetRecommendations(_referenceConverter, "Checkout")
                        .ToList();
                    break;
                case "Wishlist":
                    response = await _trackingService.TrackWishlist(ControllerContext.HttpContext);
                    recommendations = response.GetRecommendations(_referenceConverter, "Wishlist")
                        .ToList();
                    break;
                case "Order":
                    IPurchaseOrder order = null;
                    if (_contextModeResolver.CurrentMode == ContextMode.Edit)
                    {
                        break;
                    }
                    if (int.TryParse(ControllerContext.HttpContext.Request.Query["orderNumber"].ToString(), out var orderNumber))
                    {
                        order = _confirmationService.GetOrder(orderNumber);
                    }
                    if (order == null)
                    {
                        break;
                    }
                    response = await _trackingService.TrackOrder(ControllerContext.HttpContext, order);
                    recommendations = response.GetRecommendations(_referenceConverter, "orderWidget")
                        .ToList();
                    break;
                default:
                    response = await _trackingService.TrackAttribute(ControllerContext.HttpContext, name, value);
                    recommendations = response.GetRecommendations(_referenceConverter, "attributeWidget")
                        .ToList();
                    break;
            }

            if (recommendations == null)
            {
                return new ContentResult
                {
                    Content = JsonConvert.SerializeObject(new List<Recommendation>()),
                    ContentType = "application/json",
                };
            }
            recommendations = recommendations.Take(numberOfRecs).ToList();

            return new ContentResult
            {
                Content = JsonConvert.SerializeObject(recommendations),
                ContentType = "application/json",
            };
        }
    }
}
