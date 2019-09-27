using EPiServer.Core;
using EPiServer.Web.Routing;
using System.Web;

namespace Foundation.Cms.Extensions
{
    public static class UrlResolverExtensions
    {
        public static string GetUrl(this UrlResolver urlResolver, HttpRequestBase request, ContentReference contentLink,
            string language)
        {
            if (!ContentReference.IsNullOrEmpty(contentLink))
            {
                return urlResolver.GetUrl(contentLink, language);
            }

            return request.UrlReferrer == null ? "/" : request.UrlReferrer.PathAndQuery;
        }
    }
}