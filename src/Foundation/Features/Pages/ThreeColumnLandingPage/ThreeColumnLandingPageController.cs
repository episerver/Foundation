using EPiServer.Tracking.PageView;
using EPiServer.Web.Mvc;
using Foundation.Cms.ViewModels;
using System.Web.Mvc;

namespace Foundation.Features.Pages.ThreeColumnLandingPage
{
    public class ThreeColumnLandingPageController : PageController<Cms.Pages.ThreeColumnLandingPage>
    {
        [PageViewTracking]
        public ActionResult Index(Cms.Pages.ThreeColumnLandingPage currentPage)
        {
            var model = ContentViewModel.Create(currentPage);
            return View(model);
        }
    }
}