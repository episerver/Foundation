using EPiServer;
using EPiServer.Web.Routing;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Models.Catalog;
using Foundation.Commerce.Personalization;
using Foundation.Demo.ViewModels;
using Foundation.Features.CatalogContent;
using Foundation.Find.Cms.ViewModels;
using Foundation.Find.Commerce.ViewModels;
using Foundation.Social.Services;
using Mediachase.Commerce.Catalog;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Search.Category
{
    public class CategoryController : CatalogContentControllerBase<GenericNode>
    {
        private readonly ISearchViewModelFactory _viewModelFactory;

        public CategoryController(
            ISearchViewModelFactory viewModelFactory,
            ICommerceTrackingService recommendationService,
            IReviewService reviewService,
            IReviewActivityService reviewActivityService,
            ReferenceConverter referenceConverter,
            IContentLoader contentLoader,
            UrlResolver urlResolver,
            ILoyaltyService loyaltyService) : base(referenceConverter, contentLoader, urlResolver, reviewService, reviewActivityService, recommendationService, loyaltyService)
        {
            _viewModelFactory = viewModelFactory;
        }

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public async Task<ViewResult> Index(GenericNode currentContent, CommerceFilterOptionViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ViewSwitcher))
            {
                viewModel.ViewSwitcher = string.IsNullOrEmpty(currentContent.DefaultTemplate) ? "Grid" : currentContent.DefaultTemplate;
            }

            var model = _viewModelFactory.Create<DemoSearchViewModel<GenericNode>, GenericNode>(currentContent, new CommerceArgs
            {
                FilterOption = viewModel,
                SelectedFacets = HttpContext.Request.QueryString["facets"],
                CatalogId = 0
            });
            if (HttpContext.Request.HttpMethod == "GET")
            {
                var response = await _recommendationService.TrackCategory(HttpContext, currentContent);
                model.Recommendations = response.GetCategoryRecommendations(_referenceConverter);
            }

            model.BreadCrumb = GetBreadCrumb(currentContent.Code);
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult Facet(GenericNode currentContent, CommerceFilterOptionViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ViewSwitcher))
            {
                viewModel.ViewSwitcher = string.IsNullOrEmpty(currentContent.DefaultTemplate) ? "Grid" : currentContent.DefaultTemplate;
            }

            return PartialView("_Facet", viewModel);
        }
    }
}