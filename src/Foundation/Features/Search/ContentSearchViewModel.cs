using EPiServer.Find.UnifiedSearch;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using System.Net;

namespace Foundation.Features.Search
{
    public class ContentSearchViewModel
    {
        public UnifiedSearchResults Hits { get; set; }
        public FilterOptionViewModel FilterOption { get; set; }

        public string SectionFilter
        {
            get
            {
                var accessor = ServiceLocator.Current.GetInstance<IHttpContextAccessor>();
                if (accessor.HttpContext == null)
                {
                    return string.Empty;
                }

                return accessor.HttpContext.Request.Query["t"].ToString();
            }
        }

        public string GetSectionGroupUrl(string groupName)
        {
            var accessor = ServiceLocator.Current.GetInstance<IHttpContextAccessor>();
            if (accessor.HttpContext == null)
            {
                return string.Empty;
            }
            string url = UriUtil.AddQueryString(accessor.HttpContext.Request.GetDisplayUrl(), "t", WebUtility.UrlEncode(groupName));
            url = UriUtil.AddQueryString(url, "p", "1");
            return url;
        }
    }
}