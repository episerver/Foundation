using EPiServer.Web.Mvc;
using Foundation.Cms.ViewModels;
using System.Web.Mvc;

namespace Foundation.Features.Pages.TwoColumnLandingPage
{
    public class TwoColumnLandingPageController : PageController<Cms.Pages.TwoColumnLandingPage>
    {
        public ActionResult Index(Cms.Pages.TwoColumnLandingPage currentPage)
        {
            var model = ContentViewModel.Create(currentPage);
            return View(model);
        }
    }
}