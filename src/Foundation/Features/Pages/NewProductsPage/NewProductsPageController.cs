using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using Foundation.Cms.Personalization;
using Foundation.Commerce.ViewModels;
using Foundation.Demo.Models;
using Foundation.Find.Commerce;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Pages.NewProductsPage
{
    public class NewProductsPageController : PageController<Commerce.Models.Pages.NewProductsPage>
    {
        private readonly ICommerceSearchService _searchService;
        private readonly ICmsTrackingService _trackingService;
        private readonly IContentLoader _contentLoader;

        public NewProductsPageController(ICommerceSearchService searchService,
            ICmsTrackingService trackingService,
            IContentLoader contentLoader)
        {
            _searchService = searchService;
            _trackingService = trackingService;
            _contentLoader = contentLoader;
        }

        public async Task<ActionResult> Index(Commerce.Models.Pages.NewProductsPage currentPage)
        {
            await _trackingService.PageViewed(HttpContext, currentPage);
            var startPage = _contentLoader.Get<DemoHomePage>(ContentReference.StartPage);
            var model = new NewProductsPageViewModel(currentPage);
            model.ProductViewModels = _searchService.SearchNewProducts(currentPage, startPage.SearchCatalog);
            return View(model);
        }
    }
}