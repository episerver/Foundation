using EPiServer.Web.Mvc;
using Foundation.Cms.Personalization;
using Foundation.Commerce.ViewModels;
using Foundation.Find.Commerce;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Pages.SalesPage
{
    public class SalesPageController : PageController<Commerce.Models.Pages.SalesPage>
    {
        private readonly ICommerceSearchService _searchService;
        private readonly ICmsTrackingService _trackingService;

        public SalesPageController(ICommerceSearchService searchService,
            ICmsTrackingService trackingService)
        {
            _searchService = searchService;
            _trackingService = trackingService;
        }

        public async Task<ActionResult> Index(Commerce.Models.Pages.SalesPage currentPage)
        {
            await _trackingService.PageViewed(HttpContext, currentPage);
            var model = new SalesPageViewModel(currentPage);
            model.ProductViewModels = _searchService.SearchOnSale(currentPage);
            return View(model);
        }
    }
}