using EPiServer.Web.Mvc;
using Foundation.Features.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.LandingPages.LandingPage
{
    public class LandingPageController : PageController<LandingPage>
    {
        public ActionResult Index(LandingPage currentPage)
        {
            var model = ContentViewModel.Create(currentPage);
            return View(model);
        }
    }
}