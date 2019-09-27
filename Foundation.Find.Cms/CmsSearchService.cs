using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework.Statistics;
using EPiServer.Find.UnifiedSearch;
using EPiServer.Globalization;
using EPiServer.Web;
using Foundation.Cms.Extensions;
using Foundation.Cms.Pages;
using Foundation.Find.Cms.ViewModels;
using System.Linq;

namespace Foundation.Find.Cms
{
    public class CmsSearchService : ICmsSearchService
    {
        private readonly IClient _findClient;

        public CmsSearchService(IClient findClient)
        {
            _findClient = findClient;
        }

        public UnifiedSearchResults SearchContent(CmsFilterOptionViewModel filterOptions)
        {
            if (filterOptions.Q.IsNullOrEmpty())
            {
                return null;
            }

            var siteId = SiteDefinition.Current.Id;
            var query = _findClient.UnifiedSearchFor(filterOptions.Q, _findClient.Settings.Languages.GetSupportedLanguage(ContentLanguage.PreferredCulture) ?? Language.None)
                .UsingSynonyms()
                .TermsFacetFor(x => x.SearchSection)
                .FilterFacet("AllSections", x => x.SearchSection.Exists())
                .Filter(x => (x.MatchTypeHierarchy(typeof(FoundationPageData)) & (((FoundationPageData)x).SiteId().Match(siteId.ToString()))) | !x.MatchTypeHierarchy(typeof(FoundationPageData)))
                .Skip((filterOptions.Page - 1) * filterOptions.PageSize)
                .Take(filterOptions.PageSize)
                .ApplyBestBets();

            //Exclude content from search
            query = query.Filter(x => (x as FoundationPageData).ExcludeFromSearch.Match(false));

            // obey DNT
            var doNotTrackHeader = System.Web.HttpContext.Current.Request.Headers.Get("DNT");
            if ((doNotTrackHeader == null || doNotTrackHeader.Equals("0")) && filterOptions.TrackData)
            {
                query = query.Track();
            }

            if (!string.IsNullOrWhiteSpace(filterOptions.SectionFilter))
            {
                query = query.FilterHits(x => x.SearchSection.Match(filterOptions.SectionFilter));
            }

            var hitSpec = new HitSpecification
            {
                HighlightTitle = true,
                HighlightExcerpt = true
            };

            var results = query.GetResult(hitSpec);
            filterOptions.TotalCount = results?.Count() ?? 0;
            return results;
        }
    }
}
