using EPiServer.ServiceLocation;
using Microsoft.AspNetCore.Http;

namespace Foundation.Infrastructure.Cms
{
    public static class TrackingCookieManager
    {
        public static string TrackingCookieName = "_madid";

        public static string GetTrackingCookie()
        {
            var accessor = ServiceLocator.Current.GetInstance<IHttpContextAccessor>();
            if (accessor.HttpContext == null)
            {
                return string.Empty;
            }

            var cookie = accessor.HttpContext.Request.Cookies[TrackingCookieName];
            return cookie == null ? string.Empty : cookie;
        }

        public static void SetTrackingCookie(string value)
        {
            var accessor = ServiceLocator.Current.GetInstance<IHttpContextAccessor>();
            if (accessor.HttpContext == null)
            {
                return;
            }

            accessor.HttpContext.Response.Cookies.Append(TrackingCookieName, value);
        }
    }
}
