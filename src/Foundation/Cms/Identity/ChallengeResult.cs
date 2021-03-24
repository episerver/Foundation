using Microsoft.Owin.Security;
using System.Web;
using System.Web.Mvc;

namespace Foundation.Cms.Identity
{
    /// <summary>
    ///     Result class used for authenticating users against external login providers such as Facebook
    ///     and Google+.
    /// </summary>
    public class ChallengeResult : HttpUnauthorizedResult
    {
        private const string XsrfKey = "XsrfId";

        public ChallengeResult(string provider, string redirectUri)
            : this(provider, redirectUri, null)
        {
        }

        public ChallengeResult(string provider, string redirectUri, string userId)
        {
            LoginProvider = provider;
            RedirectUri = redirectUri;
            UserId = userId;
        }

        public string LoginProvider { get; set; }

        /// <summary>
        ///     Gets or sets the Uri of the callback action method that should handle that response of the provider.
        /// </summary>
        public string RedirectUri { get; set; }

        public string UserId { get; set; }

        /// <summary>
        ///     Calls the external provider and tries to authenticate the user.
        /// </summary>
        /// <param name="context">
        ///     Encapsulated information about an HTTP request that matches
        ///     specified System.Web.Routing.RouteBase and System.Web.Mvc.ControllerBase instances.
        /// </param>
        public override void ExecuteResult(ControllerContext context)
        {
            var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
            if (UserId != null)
            {
                properties.Dictionary[XsrfKey] = UserId;
            }

            context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
        }
    }
}