using System.Web.Mvc;

namespace Foundation.Attributes
{
    public class OnlyAnonymousAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(AuthorizationContext context)
        {
            if (context.HttpContext.User.Identity.IsAuthenticated)
            {
                context.HttpContext.Response.Redirect("/");
            }
        }
    }
}