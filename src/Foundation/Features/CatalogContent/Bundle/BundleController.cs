using EPiServer.Tracking.Commerce;
using Foundation.Features.CatalogContent.Variation;
using Foundation.Infrastructure.Cms;
using Foundation.Infrastructure.Commerce.Customer.Services;
using Foundation.Infrastructure.Personalization;

namespace Foundation.Features.CatalogContent.Bundle
{
    public class BundleController : CatalogContentControllerBase<GenericBundle>
    {
        private readonly bool _isInEditMode;
        private readonly CatalogEntryViewModelFactory _viewModelFactory;

        public BundleController(IsInEditModeAccessor isInEditModeAccessor,
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
        [CommerceTracking(TrackingType.Product)]
        public async Task<ActionResult> Index(GenericBundle currentContent, bool skipTracking = false)
        {
            var viewModel = _viewModelFactory.CreateBundle<GenericBundle, GenericVariant, DemoGenericBundleViewModel>(currentContent);
            viewModel.BreadCrumb = GetBreadCrumb(currentContent.Code);
            if (_isInEditMode && !viewModel.Entries.Any())
            {
                return View(viewModel);
            }

            if (viewModel.Entries == null || !viewModel.Entries.Any())
            {
                return NotFound();
            }

            await AddInfomationViewModel(viewModel, currentContent.Code, skipTracking);
            currentContent.AddBrowseHistory();

            return View(viewModel);
        }

        [HttpGet]
        public ActionResult QuickView(string productCode)
        {
            var currentContentRef = _referenceConverter.GetContentLink(productCode);
            var currentContent = _contentLoader.Get<EntryContentBase>(currentContentRef) as GenericBundle;
            if (currentContent != null)
            {
                var viewModel = _viewModelFactory.CreateBundle<GenericBundle, GenericVariant, GenericBundleViewModel>(currentContent);
                return PartialView("_QuickView", viewModel);
            }

            return StatusCode(404, "Product not found.");
        }
    }
}