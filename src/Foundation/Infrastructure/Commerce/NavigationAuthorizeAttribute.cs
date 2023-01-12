using Foundation.Infrastructure.Commerce.Customer;
using Mediachase.Commerce.Customers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Foundation.Infrastructure.Commerce
{
    public sealed class NavigationAuthorizeAttribute : ActionFilterAttribute
    {
        private List<B2BUserRoles> _authorizedRoles;

        public NavigationAuthorizeAttribute(string authorizedRoles) => ToB2BRoles(authorizedRoles);

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (ValidateUserRole())
            {
                return;
            }

            var url = new UrlHelper(filterContext);
            var redirectUrl = url.Action(new UrlActionContext { Action = "Index", Controller = "User" });
            filterContext.Result = new RedirectResult(redirectUrl);
        }

        private bool ValidateUserRole()
        {
            var contactRole = new FoundationContact(CustomerContext.Current.CurrentContact).B2BUserRole;
            return _authorizedRoles.Any(role => contactRole == role);
        }

        private void ToB2BRoles(string authorizedRoles)
        {
            _authorizedRoles = new List<B2BUserRoles>();
            var roles = authorizedRoles.Split(',');
            foreach (var role in roles)
            {
                B2BUserRoles b2BRole;
                switch (role)
                {
                    case "Admin":
                        b2BRole = B2BUserRoles.Admin;
                        break;
                    case "Approver":
                        b2BRole = B2BUserRoles.Approver;
                        break;
                    case "Purchaser":
                        b2BRole = B2BUserRoles.Purchaser;
                        break;
                    default:
                        b2BRole = B2BUserRoles.None;
                        break;
                }

                _authorizedRoles.Add(b2BRole);
            }
        }
    }
}