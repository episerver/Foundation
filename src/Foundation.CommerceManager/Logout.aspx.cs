using EPiServer.Cms.UI.AspNetIdentity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Web;
using System.Web.UI;

namespace Foundation.CommerceManager
{
    public partial class Logout : System.Web.UI.Page
    {
        /// <summary>
        /// Handles the Load event of the Page control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.Header.DataBind();

            Request.GetOwinContext().Get<ApplicationSignInManager<SiteUser>>().SignOut();
        }
    }
}
