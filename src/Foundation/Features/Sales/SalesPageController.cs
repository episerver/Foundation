using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Foundation.Features.Home;
using Foundation.Features.Search;
using System.Web.Mvc;

namespace Foundation.Features.Sales
{
    public class SalesPageController : PageController<SalesPage>
    {
        private readonly ISearchService _searchService;
        private readonly IContentLoader _contentLoader;

        public SalesPageController(ISearchService searchService, IContentLoader contentLoader)
        {
            _searchService = searchService;
            _contentLoader = contentLoader;
        }

        public ActionResult Index(SalesPage currentPage, int page = 1)
        {
            var startPage = _contentLoader.Get<HomePage>(ContentReference.StartPage);
            var model = new SalesPageViewModel(currentPage)
            {
                ProductViewModels = _searchService.SearchOnSale(currentPage, out var pages, startPage.SearchCatalog, page, 12),
                PageNumber = page,
                Pages = pages
            };

            return View(model);
        }
    }
}