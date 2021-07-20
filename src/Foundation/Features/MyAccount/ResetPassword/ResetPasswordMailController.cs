using EPiServer.Web.Mvc;
using System.Web.Mvc;

namespace Foundation.Features.MyAccount.ResetPassword
{
    public class ResetPasswordMailController : PageController<ResetPasswordMailPage>
    {
        public ActionResult Index(ResetPasswordMailPage currentPage)
        {
            var model = new ResetPasswordMailViewModel(currentPage);
            return View("ResetPasswordMail", model);
        }
    }
}