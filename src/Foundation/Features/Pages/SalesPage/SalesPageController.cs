using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Foundation.Commerce.ViewModels;
using Foundation.Demo.Models;
using Foundation.Find.Commerce;
using System.Web.Mvc;

namespace Foundation.Features.Pages.SalesPage
{
    public class SalesPageController : PageController<Commerce.Models.Pages.SalesPage>
    {
        private readonly ICommerceSearchService _searchService;
        private readonly IContentLoader _contentLoader;

        public SalesPageController(ICommerceSearchService searchService, IContentLoader contentLoader)
        {
            _searchService = searchService;
            _contentLoader = contentLoader;
        }

        public ActionResult Index(Commerce.Models.Pages.SalesPage currentPage, int page = 1)
        {
            var startPage = _contentLoader.Get<DemoHomePage>(ContentReference.StartPage);
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