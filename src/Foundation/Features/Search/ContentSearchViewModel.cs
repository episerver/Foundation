using EPiServer.Find.UnifiedSearch;
using EPiServer.Web;
using System.Web;

namespace Foundation.Features.Search
{
    public class ContentSearchViewModel
    {
        public UnifiedSearchResults Hits { get; set; }
        public FilterOptionViewModel FilterOption { get; set; }
        public string SectionFilter => HttpContext.Current.Request.QueryString["t"] ?? string.Empty;
        public string GetSectionGroupUrl(string groupName)
        {
            var url = UriUtil.AddQueryString(HttpContext.Current.Request.RawUrl, "t", HttpContext.Current.Server.UrlEncode(groupName));
            url = UriUtil.AddQueryString(url, "p", "1");
            return url;
        }
    }
}
