using EPiServer.Web.Mvc;
using Foundation.Cms.Personalization;
using Foundation.Cms.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Pages.LandingPage
{
    public class LandingPageController : PageController<Cms.Pages.LandingPage>
    {
        private readonly ICmsTrackingService _trackingService;

        public LandingPageController(ICmsTrackingService trackingService)
        {
            _trackingService = trackingService;
        }

        public async Task<ActionResult> Index(Cms.Pages.LandingPage currentPage)
        {
            await _trackingService.PageViewed(HttpContext, currentPage);
            var model = ContentViewModel.Create(currentPage);
            return View(model);
        }
    }
}