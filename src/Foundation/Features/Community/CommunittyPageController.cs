using EPiServer.Web.Mvc;
using Foundation.Features.Shared;
using System.Web.Mvc;

namespace Foundation.Features.Community
{
    public class CommunittyPageController : PageController<CommunityPage>
    {
        public ActionResult Index(CommunityPage currentPage)
        {
            var model = new ContentViewModel<CommunityPage>(currentPage);
            return View(model);
        }
    }
}