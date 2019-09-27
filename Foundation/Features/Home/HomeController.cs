using EPiServer.Web.Mvc;
using Foundation.Cms.Personalization;
using Foundation.Cms.ViewModels;
using Foundation.Demo.Models;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Home
{
    public class HomeController : PageController<DemoHomePage>
    {
        private readonly ICmsTrackingService _trackingService;

        public HomeController(ICmsTrackingService trackingService)
        {
            _trackingService = trackingService;
        }

        public async Task<ActionResult> Index(DemoHomePage currentContent)
        {
            await _trackingService.PageViewed(HttpContext, currentContent);
            return View(ContentViewModel.Create<DemoHomePage>(currentContent));
        }
    }
}