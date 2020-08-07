using EPiServer.Commerce.Order;
using EPiServer.Editor;
using EPiServer.Framework.Web.Resources;
using EPiServer.Personalization.Commerce.Tracking;
using EPiServer.Tracking.Commerce.Data;
using EPiServer.Web.Mvc;
using Foundation.Features.CatalogContent.Services;
using Foundation.Features.Checkout.Services;
using Foundation.Features.Shared;
using Foundation.Personalization;
using Mediachase.Commerce.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Recommendations.WidgetBlock
{
    public class WidgetBlockController : BlockController<WidgetBlock>
    {
        private readonly ICommerceTrackingService _trackingService;
        private readonly ReferenceConverter _referenceConverter;
        private readonly IRequiredClientResourceList _requiredClientResource;
        private readonly ICartService _cartService;
        private readonly ConfirmationService _confirmationService;
        private readonly IProductService _productService;

        public WidgetBlockController(ICommerceTrackingService commerceTrackingService,
            ReferenceConverter referenceConverter,
            IRequiredClientResourceList requiredClientResource,
            ICartService cartService,
            ConfirmationService confirmationService,
            IProductService productService)
        {
            _trackingService = commerceTrackingService;
            _referenceConverter = referenceConverter;
            _requiredClientResource = requiredClientResource;
            _cartService = cartService;
            _confirmationService = confirmationService;
            _productService = productService;
        }

        public override ActionResult Index(WidgetBlock currentContent)
        {
            return PartialView(new BlockViewModel<WidgetBlock>(currentContent));
        }

        public async Task<ActionResult> GetRecommendations(string widgetType, string name, string value = "", int numberOfRecs = 4)
        {
            if (string.IsNullOrEmpty(widgetType) || PageEditing.PageIsInEditMode)
            {
                return new EmptyResult();
            }

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
                    if (PageEditing.PageIsInEditMode)
                    {
                        break;
                    }
                    if (int.TryParse(ControllerContext.HttpContext.Request.QueryString["orderNumber"], out var orderNumber))
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
                return new EmptyResult();
            }
            recommendations = recommendations.Take(numberOfRecs).ToList();

            return PartialView("/Features/Recommendations/Index.cshtml", _productService.GetRecommendedProductTileViewModels(recommendations));
        }
    }
}
