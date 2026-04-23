// EPiServer.Find.UnifiedSearch removed: UnifiedSearchResults replaced with IEnumerable<UnifiedSearchHit> stub.
// Phase 4 will restore full Find/Graph content search.

namespace Foundation.Features.Search
{
    public class ContentSearchViewModel
    {
        public IEnumerable<UnifiedSearchHit> Hits { get; set; }
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
