using EPiServer.Web.Mvc;
using Foundation.Features.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.LandingPages.ThreeColumnLandingPage
{
    public class ThreeColumnLandingPageController : PageController<ThreeColumnLandingPage>
    {
        public ActionResult Index(ThreeColumnLandingPage currentPage)
        {
            var model = ContentViewModel.Create(currentPage);
            return View(model);
        }
    }
}