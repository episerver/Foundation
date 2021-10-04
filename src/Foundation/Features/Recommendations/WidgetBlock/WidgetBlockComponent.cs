using EPiServer.Framework.Web.Resources;
using EPiServer.Web.Mvc;
using Foundation.Features.CatalogContent.Services;
using Foundation.Features.Checkout.Services;
using Foundation.Features.Shared;
using Foundation.Infrastructure.Personalization;
using Mediachase.Commerce.Catalog;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Foundation.Features.Recommendations.WidgetBlock
{
    public class WidgetBlockComponent : AsyncBlockComponent<WidgetBlock>
    {
        private readonly ICommerceTrackingService _trackingService;
        private readonly ReferenceConverter _referenceConverter;
        private readonly IRequiredClientResourceList _requiredClientResource;
        private readonly ICartService _cartService;
        private readonly IConfirmationService _confirmationService;
        private readonly IProductService _productService;

        public WidgetBlockComponent(ICommerceTrackingService commerceTrackingService,
            ReferenceConverter referenceConverter,
            IRequiredClientResourceList requiredClientResource,
            ICartService cartService,
            IConfirmationService confirmationService,
            IProductService productService)
        {
            _trackingService = commerceTrackingService;
            _referenceConverter = referenceConverter;
            _requiredClientResource = requiredClientResource;
            _cartService = cartService;
            _confirmationService = confirmationService;
            _productService = productService;
        }

        protected override async Task<IViewComponentResult> InvokeComponentAsync(WidgetBlock currentBlock)
        {
            return await Task.FromResult(View("/Features/Recommendations/WidgetBlock/Index.cshtml", new BlockViewModel<WidgetBlock>(currentBlock)));
        }
    }
}
