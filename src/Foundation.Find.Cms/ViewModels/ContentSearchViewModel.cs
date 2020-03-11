using EPiServer.Find.UnifiedSearch;
using EPiServer.Web;
using Foundation.Find.Cms.Facets;
using System.Collections.Generic;
using System.Web;

namespace Foundation.Find.Cms.ViewModels
{
    public class ContentSearchViewModel
    {
        public UnifiedSearchResults Hits { get; set; }
        public CmsFilterOptionViewModel FilterOption { get; set; }
        public string SectionFilter => HttpContext.Current.Request.QueryString["t"] ?? string.Empty;
        public List<FacetGroupOption> FacetGroups { get; set; }
        public string ContentFacet => HttpContext.Current.Request.QueryString["c"] ?? string.Empty;

        public string GetSectionGroupUrl(string groupName)
        {
            string url = UriUtil.AddQueryString(HttpContext.Current.Request.RawUrl, "t", HttpContext.Current.Server.UrlEncode(groupName));
            url = UriUtil.AddQueryString(url, "c", "");
            url = UriUtil.AddQueryString(url, "p", "1");
            return url;
        }

        public string GetContentFacetUrl(string facet)
        {
            string url = UriUtil.AddQueryString(HttpContext.Current.Request.RawUrl, "c", HttpContext.Current.Server.UrlEncode(facet));
            url = UriUtil.AddQueryString(url, "t", "");
            url = UriUtil.AddQueryString(url, "p", "1");
            return url;
        }

    }
}
