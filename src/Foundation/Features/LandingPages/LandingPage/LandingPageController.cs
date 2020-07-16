using EPiServer.Tracking.PageView;
using EPiServer.Web.Mvc;
using Foundation.Features.Shared;
using System.Web.Mvc;

namespace Foundation.Features.LandingPages.LandingPage
{
    public class LandingPageController : PageController<LandingPage>
    {
        [PageViewTracking]
        public ActionResult Index(LandingPage currentPage)
        {
            var model = ContentViewModel.Create(currentPage);
            return View(model);
        }
    }
}