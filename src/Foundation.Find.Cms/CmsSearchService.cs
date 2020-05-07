using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework.Statistics;
using EPiServer.Find.UnifiedSearch;
using EPiServer.Globalization;
using EPiServer.Web;
using Foundation.Cms.Extensions;
using Foundation.Cms.Media;
using Foundation.Cms.Pages;
using Foundation.Find.Cms.ViewModels;
using Geta.EpiCategories;
using Geta.EpiCategories.Find.Extensions;
using System.Collections.Generic;

namespace Foundation.Find.Cms
{
    public class CmsSearchService : ICmsSearchService
    {
        private readonly IClient _findClient;

        public CmsSearchService(IClient findClient) => _findClient = findClient;

        public ContentSearchViewModel SearchContent(CmsFilterOptionViewModel filterOptions)
        {
            var model = new ContentSearchViewModel
            {
                FilterOption = filterOptions
            };

            if (!filterOptions.Q.IsNullOrEmpty())
            {
                var siteId = SiteDefinition.Current.Id;
                var query = _findClient.UnifiedSearchFor(filterOptions.Q, _findClient.Settings.Languages.GetSupportedLanguage(ContentLanguage.PreferredCulture) ?? Language.None)
                    .UsingSynonyms()
                    .TermsFacetFor(x => x.SearchSection)
                    .FilterFacet("AllSections", x => x.SearchSection.Exists())
                    .Filter(x => (x.MatchTypeHierarchy(typeof(FoundationPageData)) & (((FoundationPageData)x).SiteId().Match(siteId.ToString())) | (x.MatchTypeHierarchy(typeof(PageData)) & x.MatchTypeHierarchy(typeof(MediaData)))))
                    .Skip((filterOptions.Page - 1) * filterOptions.PageSize)
                    .Take(filterOptions.PageSize)
                    .ApplyBestBets();

                //Include images in search results
                if (!filterOptions.IncludeImagesContent)
                {
                    query = query.Filter(x => !x.MatchType(typeof(ImageMediaData)));
                }

                //Exclude content from search
                query = query.Filter(x => !(x as FoundationPageData).ExcludeFromSearch.Exists() | (x as FoundationPageData).ExcludeFromSearch.Match(false));

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

                model.Hits = query.GetResult(hitSpec);
                filterOptions.TotalCount = model.Hits.TotalMatching;
            }

            return model;
        }

        public ContentSearchViewModel SearchPdf(CmsFilterOptionViewModel filterOptions)
        {
            var model = new ContentSearchViewModel
            {
                FilterOption = filterOptions
            };

            if (!filterOptions.Q.IsNullOrEmpty())
            {
                var siteId = SiteDefinition.Current.Id;
                var query = _findClient.UnifiedSearchFor(filterOptions.Q, _findClient.Settings.Languages.GetSupportedLanguage(ContentLanguage.PreferredCulture) ?? Language.None)
                    .UsingSynonyms()
                    .TermsFacetFor(x => x.SearchSection)
                    .FilterFacet("AllSections", x => x.SearchSection.Exists())
                    .Filter(x => x.MatchType(typeof(FoundationPdfFile)))
                    .Skip((filterOptions.Page - 1) * filterOptions.PageSize)
                    .Take(filterOptions.PageSize)
                    .ApplyBestBets();

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

                model.Hits = query.GetResult(hitSpec);
                filterOptions.TotalCount = model.Hits.TotalMatching;
            }

            return model;
        }

        public CategorySearchResults SearchByCategory(Pagination pagination)
        {
            if (pagination == null)
            {
                pagination = new Pagination();
            }

            var query = _findClient.Search<FoundationPageData>();
            query = query.FilterByCategories(pagination.Categories);

            if (pagination.Sort == CategorySorting.PublishedDate.ToString())
            {
                if (pagination.SortDirection.ToLower() == "asc")
                    query = query.OrderBy(x => x.StartPublish);
                else
                    query = query.OrderByDescending(x => x.StartPublish);
            }

            if (pagination.Sort == CategorySorting.Name.ToString())
            {
                if (pagination.SortDirection.ToLower() == "asc")
                    query = query.OrderBy(x => x.Name);
                else
                    query = query.OrderByDescending(x => x.Name);
            }

            query = query.Skip((pagination.Page - 1) * pagination.PageSize).Take(pagination.PageSize);
            var results = query.GetContentResult();
            var model = new CategorySearchResults();
            model.Pagination = pagination;
            model.RelatedPages = results;
            model.Pagination.TotalMatching = results.TotalMatching;
            model.Pagination.TotalPage = model.Pagination.TotalMatching / pagination.PageSize + (model.Pagination.TotalMatching % pagination.PageSize > 0 ? 1 : 0);

            return model;
        }

        public ITypeSearch<T> FilterByCategories<T>(ITypeSearch<T> query, IEnumerable<ContentReference> categories) where T : ICategorizableContent
        {
            return query.FilterByCategories(categories);
        }
    }
}
