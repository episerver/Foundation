using EPiServer.Tracking.PageView;
using EPiServer.Web.Mvc;
using Foundation.Features.Shared;
using System.Web.Mvc;

namespace Foundation.Features.LandingPages.TwoColumnLandingPage
{
    public class TwoColumnLandingPageController : PageController<TwoColumnLandingPage>
    {
        [PageViewTracking]
        public ActionResult Index(TwoColumnLandingPage currentPage)
        {
            var model = ContentViewModel.Create(currentPage);
            return View(model);
        }
    }
}