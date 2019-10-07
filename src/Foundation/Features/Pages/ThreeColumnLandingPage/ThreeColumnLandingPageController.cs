using EPiServer.Web.Mvc;
using Foundation.Cms.Personalization;
using Foundation.Cms.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Pages.ThreeColumnLandingPage
{
    public class ThreeColumnLandingPageController : PageController<Cms.Pages.ThreeColumnLandingPage>
    {
        private readonly ICmsTrackingService _trackingService;

        public ThreeColumnLandingPageController(ICmsTrackingService trackingService)
        {
            _trackingService = trackingService;
        }
        public async Task<ActionResult> Index(Cms.Pages.ThreeColumnLandingPage currentPage)
        {
            await _trackingService.PageViewed(HttpContext, currentPage);
            var model = ContentViewModel.Create(currentPage);
            return View(model);
        }
    }
}