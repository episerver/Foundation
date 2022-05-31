using System.Security.Principal;

namespace Foundation.Social
{
    public static class IPrincipalExtensions
    {
        public static string GetUserId(IPrincipal user)
        {
            var userId = user.Identity.Name;
            if (string.IsNullOrWhiteSpace(userId))
            {
                return string.Empty;
            }

            return userId;
        }
    }
}
