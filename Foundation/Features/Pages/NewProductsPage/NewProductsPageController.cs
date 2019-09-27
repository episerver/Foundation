using EPiServer.Web.Mvc;
using Foundation.Cms.Personalization;
using Foundation.Commerce.ViewModels;
using Foundation.Find.Commerce;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Pages.NewProductsPage
{
    public class NewProductsPageController : PageController<Commerce.Models.Pages.NewProductsPage>
    {
        private readonly ICommerceSearchService _searchService;
        private readonly ICmsTrackingService _trackingService;

        public NewProductsPageController(ICommerceSearchService searchService,
            ICmsTrackingService trackingService)
        {
            _searchService = searchService;
            _trackingService = trackingService;
        }

        public async Task<ActionResult> Index(Commerce.Models.Pages.NewProductsPage currentPage)
        {
            await _trackingService.PageViewed(HttpContext, currentPage);
            var model = new NewProductsPageViewModel(currentPage);
            model.ProductViewModels = _searchService.SearchNewProducts(currentPage);
            return View(model);
        }
    }
}