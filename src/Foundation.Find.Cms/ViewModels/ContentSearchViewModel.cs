using EPiServer.Find.UnifiedSearch;
using EPiServer.Web;
using System.Web;

namespace Foundation.Find.Cms.ViewModels
{
    public class ContentSearchViewModel
    {
        public UnifiedSearchResults Hits { get; set; }
        public CmsFilterOptionViewModel FilterOption { get; set; }
        public string SectionFilter => HttpContext.Current.Request.QueryString["t"] ?? string.Empty;
        public string GetSectionGroupUrl(string groupName)
        {
            string url = UriUtil.AddQueryString(HttpContext.Current.Request.RawUrl, "t", HttpContext.Current.Server.UrlEncode(groupName));
            url = UriUtil.AddQueryString(url, "p", "1");
            return url;
        }
    }
}
