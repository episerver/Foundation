using EPiServer.Core;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Http;

namespace Foundation.Infrastructure.Cms.Extensions
{
    public static class UrlResolverExtensions
    {
        public static string GetUrl(this UrlResolver urlResolver, HttpRequest request, ContentReference contentLink,
            string language)
        {
            if (!ContentReference.IsNullOrEmpty(contentLink))
            {
                return urlResolver.GetUrl(contentLink, language);
            }

            return request.GetTypedHeaders().Referer == null ? "/" : request.GetTypedHeaders().Referer.PathAndQuery;
        }
    }
}