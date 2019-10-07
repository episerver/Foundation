using Microsoft.AspNet.Identity;
using System.Security.Principal;

namespace Foundation.Social
{
    public static class IPrincipalExtensions
    {
        public static string GetUserId(IPrincipal user)
        {
            var userId = user.Identity.GetUserId();
            if (string.IsNullOrWhiteSpace(userId))
            {
                return string.Empty;
            }

            return userId;
        }
    }
}
