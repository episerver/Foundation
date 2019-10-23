using System;
using System.Web;

namespace Foundation.Cms
{
    public class CookieService
    {
        public virtual string Get(string cookie)
        {
            if (HttpContext.Current == null)
            {
                return null;
            }

            return HttpContext.Current.Request.Cookies[cookie]?.Value;
        }

        public virtual void Set(string cookie, string value, bool sessionCookie = false)
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            var httpCookie = new HttpCookie(cookie)
            {
                Value = value,
                HttpOnly = true,
                Secure = HttpContext.Current?.Request?.IsSecureConnection ?? false
            };


            if (!sessionCookie)
            {
                httpCookie.Expires = DateTime.Now.AddYears(1);
            }

            Set(HttpContext.Current.Response.Cookies, httpCookie);
            HttpContext.Current.Request.Cookies.Set(httpCookie);
        }

        public virtual void Remove(string cookie)
        {
            if (HttpContext.Current == null)
            {
                return;
            }

            var httpCookie = new HttpCookie(cookie)
            {
                Expires = DateTime.Now.AddDays(-1),
#if RELEASE
                HttpOnly = true,
                Secure = true
#endif
            };

            Set(HttpContext.Current.Response.Cookies, httpCookie);
            HttpContext.Current.Request.Cookies.Set(httpCookie);
        }

        private void Set(HttpCookieCollection cookieCollection, HttpCookie cookie) => cookieCollection.Add(cookie);
    }
}