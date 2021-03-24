using System.Web;

namespace Foundation.Cms
{
    public static class TrackingCookieManager
    {
        public static string TrackingCookieName = "_madid";

        public static string GetTrackingCookie()
        {
            if (HttpContext.Current == null)
            {
                return string.Empty;
            }

            var cookie = HttpContext.Current.Request.Cookies[TrackingCookieName];
            return cookie == null ? string.Empty : cookie.Value;
        }

        public static void SetTrackingCookie(string value)
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            if (HttpContext.Current.Request.Cookies[TrackingCookieName] == null)
            {
                HttpContext.Current.Response.Cookies.Add(new HttpCookie(TrackingCookieName));
            }

            HttpContext.Current.Response.Cookies[TrackingCookieName].Value = value;
        }
    }
}
