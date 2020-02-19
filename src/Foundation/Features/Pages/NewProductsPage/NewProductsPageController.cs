using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Foundation.Commerce.ViewModels;
using Foundation.Demo.Models;
using Foundation.Find.Commerce;
using System.Web.Mvc;

namespace Foundation.Features.Pages.NewProductsPage
{
    public class NewProductsPageController : PageController<Commerce.Models.Pages.NewProductsPage>
    {
        private readonly ICommerceSearchService _searchService;
        private readonly IContentLoader _contentLoader;

        public NewProductsPageController(ICommerceSearchService searchService, IContentLoader contentLoader)
        {
            _searchService = searchService;
            _contentLoader = contentLoader;
        }

        public ActionResult Index(Commerce.Models.Pages.NewProductsPage currentPage, int page = 1)
        {
            var startPage = _contentLoader.Get<DemoHomePage>(ContentReference.StartPage);
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