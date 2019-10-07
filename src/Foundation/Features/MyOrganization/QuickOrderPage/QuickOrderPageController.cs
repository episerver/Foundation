using EPiServer.Web.Mvc;
using Foundation.Commerce.Order.ViewModels;
using System.Web.Mvc;

namespace Foundation.Features.MyOrganization.QuickOrderPage
{
    [Authorize]
    public class QuickOrderPageController : PageController<Commerce.Models.Pages.QuickOrderPage>
    {
        public ActionResult Index(Commerce.Models.Pages.QuickOrderPage currentPage)
        {
            return View(new QuickOrderPageViewModel
            {
                CurrentContent = currentPage
            });
        }
    }
}