using EPiServer.Web.Mvc;
using Foundation.Cms.Personalization;
using Foundation.Cms.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Pages.TwoColumnLandingPage
{
    public class TwoColumnLandingPageController : PageController<Cms.Pages.TwoColumnLandingPage>
    {
        private readonly ICmsTrackingService _trackingService;

        public TwoColumnLandingPageController(ICmsTrackingService trackingService)
        {
            _trackingService = trackingService;
        }

        public async Task<ActionResult> Index(Cms.Pages.TwoColumnLandingPage currentPage)
        {
            await _trackingService.PageViewed(HttpContext, currentPage);
            var model = ContentViewModel.Create(currentPage);
            return View(model);
        }
    }
}