using Foundation.Features.Search;

namespace Foundation.Features.Category
{
    public class StandardCategoryController : ContentController<StandardCategory>
    {
        private readonly ISearchService _searchService;
        private readonly IContentLoader _contentLoader;

        public StandardCategoryController(ISearchService searchService, IContentLoader contentLoader)
        {
            _searchService = searchService;
            _contentLoader = contentLoader;
        }

        public ActionResult Index(StandardCategory currentContent, Pagination pagination)
        {
            var categories = new List<ContentReference> { currentContent.ContentLink };
            pagination.Categories = categories;
            var model = new CategorySearchViewModel(currentContent)
            {
                SearchResults = _searchService.SearchByCategory(pagination)
            };
            return View(model);
        }

        public ActionResult GetListPages(StandardCategory currentContent, Pagination pagination)
        {
            var categories = new List<ContentReference> { currentContent.ContentLink };
            pagination.Categories = categories;
            var model = new CategorySearchViewModel(currentContent)
            {
                SearchResults = _searchService.SearchByCategory(pagination)
            };
            return PartialView("_PageListing", model);
        }

    }
}