using EPiServer;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Extensions;
using Foundation.Features.CatalogContent.DynamicCatalogContent.DynamicVariation;
using Foundation.Personalization;
using Foundation.Social.Services;
using Mediachase.Commerce.Catalog;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.CatalogContent.DynamicCatalogContent.DynamicProduct
{
    public class DynamicProductController : CatalogContentControllerBase<DynamicProduct>
    {
        private readonly bool _isInEditMode;
        private readonly CatalogEntryViewModelFactory _viewModelFactory;

        public DynamicProductController(IsInEditModeAccessor isInEditModeAccessor,
            CatalogEntryViewModelFactory viewModelFactory,
            IReviewService reviewService,
            IReviewActivityService reviewActivityService,
            ICommerceTrackingService recommendationService,
            ReferenceConverter referenceConverter,
            IContentLoader contentLoader,
            UrlResolver urlResolver,
            ILoyaltyService loyaltyService) : base(referenceConverter, contentLoader, urlResolver, reviewService, reviewActivityService, recommendationService, loyaltyService)
        {
            _isInEditMode = isInEditModeAccessor();
            _viewModelFactory = viewModelFactory;
        }

        [HttpGet]
        public async Task<ActionResult> Index(DynamicProduct currentContent, string variationCode = "", bool skipTracking = false)
        {
            var viewModel = _viewModelFactory.Create<DynamicProduct, DynamicVariant, DynamicProductViewModel>(currentContent, variationCode);

            if (_isInEditMode && viewModel.Variant == null)
            {
                return View(viewModel);
            }

            if (viewModel.Variant == null)
            {
                return HttpNotFound();
            }

            viewModel.GenerateVariantGroup();
            await AddInfomationViewModel(viewModel, currentContent.Code, skipTracking);
            currentContent.AddBrowseHistory();
            viewModel.BreadCrumb = GetBreadCrumb(currentContent.Code);
            return View("", viewModel);
        }
    }
}