using Foundation.Features.CatalogContent.Variation;
using Foundation.Infrastructure.Cms;
using Foundation.Infrastructure.Commerce.Customer.Services;
using Foundation.Infrastructure.Personalization;

namespace Foundation.Features.CatalogContent.Product
{
    public class ProductController : CatalogContentControllerBase<GenericProduct>
    {
        private readonly bool _isInEditMode;
        private readonly CatalogEntryViewModelFactory _viewModelFactory;

        public ProductController(IsInEditModeAccessor isInEditModeAccessor,
            CatalogEntryViewModelFactory viewModelFactory,
            //IReviewService reviewService,
            //IReviewActivityService reviewActivityService,
            ICommerceTrackingService recommendationService,
            ReferenceConverter referenceConverter,
            IContentLoader contentLoader,
            UrlResolver urlResolver,
            ILoyaltyService loyaltyService) : base(referenceConverter, contentLoader, urlResolver, /*reviewService, reviewActivityService,*/ recommendationService, loyaltyService)
        {
            _isInEditMode = isInEditModeAccessor();
            _viewModelFactory = viewModelFactory;
        }

        [HttpGet]
        public async Task<ActionResult> Index(GenericProduct currentContent, string variationCode = "", bool skipTracking = false)
        {
            var viewModel = _viewModelFactory.Create<GenericProduct, GenericVariant, GenericProductViewModel>(currentContent, variationCode);

            if (_isInEditMode && viewModel.Variant == null)
            {
                return View(viewModel);
            }

            if (viewModel.Variant == null)
            {
                return NotFound();
            }

            await AddInfomationViewModel(viewModel, currentContent.Code, skipTracking);
            currentContent.AddBrowseHistory();
            viewModel.BreadCrumb = GetBreadCrumb(currentContent.Code);
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult QuickView(string productCode, string variantCode)
        {
            var currentContentRef = _referenceConverter.GetContentLink(productCode);
            var currentContent = _contentLoader.Get<ProductContent>(currentContentRef) as GenericProduct;
            if (currentContent != null)
            {
                var viewModel = _viewModelFactory.Create<GenericProduct, GenericVariant, GenericProductViewModel>(currentContent, variantCode);
                return PartialView("_QuickView", viewModel);
            }

            return StatusCode(404, "Product not found.");
        }

        [HttpGet]
        public ActionResult SelectVariant(string productCode, string color, string size, bool isQuickView = true)
        {
            var currentContentRef = _referenceConverter.GetContentLink(productCode);
            var currentContent = _contentLoader.Get<ProductContent>(currentContentRef) as GenericProduct;
            if (currentContent != null)
            {
                var variant = _viewModelFactory.SelectVariant(currentContent, color, size);
                if (variant != null)
                {
                    var viewModel = _viewModelFactory.Create<GenericProduct, GenericVariant, GenericProductViewModel>(currentContent, variant.Code);

                    if (isQuickView)
                    {
                        return PartialView("_QuickView", viewModel);
                    }
                    else
                    {
                        return PartialView("_ProductDetail", viewModel);
                    }
                }
            }

            return StatusCode(404, "Product not found.");
        }
    }
}