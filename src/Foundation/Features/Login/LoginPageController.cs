using EPiServer.Web.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.Login;

public class LoginPageController : PageController<LoginPage>
{
    public ActionResult Index(LoginPage currentPage)
    {
        var model = new LoginPageViewModel(currentPage);
        return View("/Features/Login/LoginPage.cshtml", model);
    }
}