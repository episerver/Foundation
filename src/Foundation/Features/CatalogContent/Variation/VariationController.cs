using EPiServer.Framework.DataAnnotations;
using Foundation.Infrastructure.Cms;
using Foundation.Infrastructure.Commerce.Customer.Services;
using Foundation.Infrastructure.Personalization;

namespace Foundation.Features.CatalogContent.Variation
{
    [TemplateDescriptor(Inherited = true)]
    public class VariationController : CatalogContentControllerBase<GenericVariant>
    {
        private readonly bool _isInEditMode;
        private readonly CatalogEntryViewModelFactory _viewModelFactory;

        public VariationController(
            IsInEditModeAccessor isInEditModeAccessor,
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
        public IActionResult Index(GenericVariant currentContent)
        {
            var viewModel = _viewModelFactory.CreateVariant<GenericVariant, GenericVariantViewModel>(currentContent);
            viewModel.BreadCrumb = GetBreadCrumb(currentContent.Code);
            return View(viewModel);
        }
    }
}