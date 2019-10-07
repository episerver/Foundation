using EPiServer.Cms.UI.AspNetIdentity;
using EPiServer.ServiceLocation;
using Foundation.Cms.Identity;
using Mediachase.BusinessFoundation;
using System.Web;

namespace Foundation.CommerceManager
{
    public class SignInAsDifferentUserHandler : ICommand
    {
        public void Invoke(object sender, object element)
        {
            ServiceLocator.Current.GetInstance<ApplicationSignInManager<SiteUser>>().SignOut();
            HttpContext.Current.Response.Redirect("~/Apps/Shell/Pages/Login.aspx");
            HttpContext.Current.Response.End();
        }
    }
}