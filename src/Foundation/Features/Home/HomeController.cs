using EPiServer.Tracking.PageView;
using EPiServer.Web.Mvc;
using Foundation.Features.Shared;
using System.Web.Mvc;

namespace Foundation.Features.Home
{
    public class HomeController : PageController<HomePage>
    {
        [PageViewTracking]
        public ActionResult Index(HomePage currentContent)
        {
            return View(ContentViewModel.Create<HomePage>(currentContent));
        }
    }
}