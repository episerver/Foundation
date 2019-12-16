using EPiServer.Web.Mvc;
using Foundation.Cms.ViewModels;
using System.Web.Mvc;

namespace Foundation.Features.Pages.CommunityPage
{
    public class CommunittyPageController : PageController<Social.Models.Pages.CommunityPage>
    {
        public ActionResult Index(Social.Models.Pages.CommunityPage currentPage)
        {
            var model = new ContentViewModel<Social.Models.Pages.CommunityPage>(currentPage);
            return View("~/Features/Pages/CommunityPage/Index.cshtml", model);
        }
    }
}