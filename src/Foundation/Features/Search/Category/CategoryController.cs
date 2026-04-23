using Foundation.Features.CatalogContent;
using Foundation.Infrastructure.Commerce.Customer.Services;
//using Foundation.Social.Services;

namespace Foundation.Features.Search.Category
{
    public class CategoryController : CatalogContentControllerBase<GenericNode>
    {
        private readonly ISearchViewModelFactory _viewModelFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CategoryController(
            ISearchViewModelFactory viewModelFactory,
            IHttpContextAccessor httpContextAccessor,
            //IReviewService reviewService,
            //IReviewActivityService reviewActivityService,
            ReferenceConverter referenceConverter,
            IContentLoader contentLoader,
            UrlResolver urlResolver,
            ILoyaltyService loyaltyService) : base(referenceConverter, contentLoader, urlResolver/*, reviewService, reviewActivityService*/, loyaltyService)
        {
            _viewModelFactory = viewModelFactory;
            _httpContextAccessor = httpContextAccessor;
        }

        [AcceptVerbs(new string[] { "GET", "POST" })]
        public ViewResult Index(GenericNode currentContent, FilterOptionViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ViewSwitcher))
            {
                viewModel.ViewSwitcher = string.IsNullOrEmpty(currentContent.DefaultTemplate) ? "Grid" : currentContent.DefaultTemplate;
            }

            var model = _viewModelFactory.Create(currentContent,
                _httpContextAccessor.HttpContext.Request.Query["facets"].ToString(),
                0,
                viewModel);

            model.BreadCrumb = GetBreadCrumb(currentContent.Code);
            return View(model);
        }

        public ActionResult Facet(GenericNode currentContent, FilterOptionViewModel viewModel)
        {
            if (string.IsNullOrEmpty(viewModel.ViewSwitcher))
            {
                viewModel.ViewSwitcher = string.IsNullOrEmpty(currentContent.DefaultTemplate) ? "Grid" : currentContent.DefaultTemplate;
            }

            return PartialView("_Facet", viewModel);
        }
    }
}
