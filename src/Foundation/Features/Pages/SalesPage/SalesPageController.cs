using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Foundation.Cms.Personalization;
using Foundation.Commerce.ViewModels;
using Foundation.Demo.Models;
using Foundation.Find.Commerce;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Pages.SalesPage
{
    public class SalesPageController : PageController<Commerce.Models.Pages.SalesPage>
    {
        private readonly ICommerceSearchService _searchService;
        private readonly ICmsTrackingService _trackingService;
        private readonly IContentLoader _contentLoader;

        public SalesPageController(ICommerceSearchService searchService,
            ICmsTrackingService trackingService,
            IContentLoader contentLoader)
        {
            _searchService = searchService;
            _trackingService = trackingService;
            _contentLoader = contentLoader;
        }

        public async Task<ActionResult> Index(Commerce.Models.Pages.SalesPage currentPage, int page = 1)
        {
            await _trackingService.PageViewed(HttpContext, currentPage);
            var startPage = _contentLoader.Get<DemoHomePage>(ContentReference.StartPage);
            var model = new SalesPageViewModel(currentPage);
            model.ProductViewModels = _searchService.SearchOnSale(currentPage, out var pages, startPage.SearchCatalog, page);
            model.PageNumber = page;
            model.Pages = pages;
            return View(model);
        }
    }
}