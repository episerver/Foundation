using EPiServer.Cms.UI.AspNetIdentity;
using Mediachase.BusinessFoundation;
using System.Web;

namespace Foundation.CommerceManager
{
    public class SignInAsDifferentUserHandler : ICommand
    {
        private readonly ApplicationSignInManager<SiteUser> _signInManager;

        public SignInAsDifferentUserHandler(ApplicationSignInManager<SiteUser> signInManager) => _signInManager = signInManager;

        public void Invoke(object sender, object element)
        {
            _signInManager.SignOut();
            HttpContext.Current.Response.Redirect("~/Apps/Shell/Pages/Login.aspx");
            HttpContext.Current.Response.End();
        }
    }
}