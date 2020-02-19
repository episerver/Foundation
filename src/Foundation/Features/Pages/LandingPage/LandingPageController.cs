using EPiServer.Web.Mvc;
using Foundation.Cms.ViewModels;
using System.Web.Mvc;

namespace Foundation.Features.Pages.LandingPage
{
    public class LandingPageController : PageController<Cms.Pages.LandingPage>
    {
        public ActionResult Index(Cms.Pages.LandingPage currentPage)
        {
            var model = ContentViewModel.Create(currentPage);
            return View(model);
        }
    }
}