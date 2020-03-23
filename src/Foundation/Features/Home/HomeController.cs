using EPiServer.Tracking.PageView;
using EPiServer.Web.Mvc;
using Foundation.Cms.ViewModels;
using Foundation.Demo.Models;
using System.Web.Mvc;

namespace Foundation.Features.Home
{
    public class HomeController : PageController<DemoHomePage>
    {
        [PageViewTracking]
        public ActionResult Index(DemoHomePage currentContent)
        {
            return View(ContentViewModel.Create<DemoHomePage>(currentContent));
        }
    }
}