using EPiServer.Tracking.PageView;
using EPiServer.Web.Mvc;
using Foundation.Features.Shared;
using System.Web.Mvc;

namespace Foundation.Features.LandingPages.ThreeColumnLandingPage
{
    public class ThreeColumnLandingPageController : PageController<ThreeColumnLandingPage>
    {
        [PageViewTracking]
        public ActionResult Index(ThreeColumnLandingPage currentPage)
        {
            var model = ContentViewModel.Create(currentPage);
            return View(model);
        }
    }
}