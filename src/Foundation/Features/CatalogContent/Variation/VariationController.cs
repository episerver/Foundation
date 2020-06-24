using EPiServer;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Commerce.Customer.Services;
using Foundation.Personalization;
using Foundation.Social.Services;
using Mediachase.Commerce.Catalog;
using System.Web.Mvc;

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
        public ActionResult Index(GenericVariant currentContent)
        {
            var viewModel = _viewModelFactory.CreateVariant<GenericVariant, GenericVariantViewModel>(currentContent);
            viewModel.BreadCrumb = GetBreadCrumb(currentContent.Code);
            return Request.IsAjaxRequest() ? PartialView(viewModel) : (ActionResult)View(viewModel);
        }
    }
}