using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Foundation.Features.Home;
using Foundation.Features.Search;
using System.Web.Mvc;

namespace Foundation.Features.NewProducts
{
    public class NewProductsPageController : PageController<NewProductsPage>
    {
        private readonly ISearchService _searchService;
        private readonly IContentLoader _contentLoader;

        public NewProductsPageController(ISearchService searchService, IContentLoader contentLoader)
        {
            _searchService = searchService;
            _contentLoader = contentLoader;
        }

        public ActionResult Index(NewProductsPage currentPage, int page = 1)
        {
            var startPage = _contentLoader.Get<HomePage>(ContentReference.StartPage);
            var model = new NewProductsPageViewModel(currentPage)
            {
                ProductViewModels = _searchService.SearchNewProducts(currentPage, out var pages, startPage.SearchCatalog, page),
                PageNumber = page,
                Pages = pages
            };

            return View(model);
        }
    }
}