using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Foundation.CommerceManager
{
    /// <summary>
    /// This is a custom code behind for the login page for CommerceManager
    /// to get OWIN authentication.
    /// </summary>
    /// <remarks>
    /// We replace the original one as a build step when building this project. 
    /// See the AfterBuild msbuild target in this files project file for details 
    /// on how that is done.
    /// </remarks>
    public partial class Login : Page
    {
        private const string UserLoginFailureMessage = "Login failed. Please try again.";
        public ApplicationSignInManager<SiteUser> SignInManager { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();

            SignInManager = Request.GetOwinContext().Get<ApplicationSignInManager<SiteUser>>();
            LoginCtrl.Authenticate += LoginCtrl_Authenticate;

            if (IsPostBack)
            {
                return;
            }
            LoginCtrl.FindControl("ApplicationRow").Visible = Mediachase.Commerce.Core.AppContext.Current.GetApplicationDto().Application.Count != 1;
            LoginCtrl.Focus();
        }

        protected void LoginCtrl_Authenticate(object sender, AuthenticateEventArgs e)
        {
            var userName = ((TextBox)LoginCtrl.FindControl("UserName")).Text;
            var password = ((TextBox)LoginCtrl.FindControl("Password")).Text;
            var remember = ((CheckBox)LoginCtrl.FindControl("RememberMe")).Checked;

            var validated = SignInManager.PasswordSignIn(userName, password, remember, false) == SignInStatus.Success;
            if (validated)
            {
                HandleLoginSuccess(userName, remember);
            }
            else
            {
                HandleLoginFailure(UserLoginFailureMessage);
            }
        }

        private void HandleLoginSuccess(string userName, bool remember)
        {
            var url = FormsAuthentication.GetRedirectUrl(userName, remember);
            if (url.Equals(FormsAuthentication.DefaultUrl, StringComparison.OrdinalIgnoreCase) ||
                url.Contains(".axd") ||
                url.Contains("/Apps/Core/Controls/Uploader/"))
            {
                url = "~/Apps/Shell/Pages/default.aspx";
            }

            Response.Redirect(url);
        }

        private void HandleLoginFailure(string pageMessage) => LoginCtrl.FailureText = pageMessage;
    }
}