using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Api.Querying.Filters;
using EPiServer.Find.Cms;
using EPiServer.Find.Framework.Statistics;
using EPiServer.Find.UnifiedSearch;
using EPiServer.Globalization;
using EPiServer.Web;
using Foundation.Cms.Extensions;
using Foundation.Cms.Media;
using Foundation.Cms.Pages;
using Foundation.Find.Cms.Facets;
using Foundation.Find.Cms.ViewModels;
using Geta.EpiCategories;
using Geta.EpiCategories.Find.Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Find.Cms
{
    public class CmsSearchService : ICmsSearchService
    {
        private readonly IClient _findClient;
        private readonly IFacetRegistry _facetRegistry;

        public CmsSearchService(IClient findClient, IFacetRegistry facetRegistry)
        {
            _findClient = findClient;
            _facetRegistry = facetRegistry;
        }

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

                var facetQuery = query;

                if (!string.IsNullOrWhiteSpace(filterOptions.SectionFilter))
                {
                    query = query.FilterHits(x => x.SearchSection.Match(filterOptions.SectionFilter));
                }
                
                if (!string.IsNullOrWhiteSpace(filterOptions.ContentFacet))
                {
                    if (!int.TryParse(filterOptions.ContentFacet, out var categoryId))
                    {
                        query = query.FilterHits(x => (x as FoundationPageData).ContentTypeName().Match(filterOptions.ContentFacet));
                    }
                    else
                    {
                        query = query.FilterHits(x => (x as FoundationPageData).Categories.MatchContained(y => y.ID, categoryId));
                    }
                }

                var hitSpec = new HitSpecification
                {
                    HighlightTitle = true,
                    HighlightExcerpt = true
                };

                model.Hits = query.GetResult(hitSpec);
                filterOptions.TotalCount = model.Hits.TotalMatching;
                model.FacetGroups = GetFacetResults(facetQuery, filterOptions.ContentFacet);
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

        private List<FacetGroupOption> GetFacetResults(ITypeSearch<ISearchContent> query,
                string selectedfacets)
        {
            var options = new List<FacetGroupOption>();
            var facets = _facetRegistry.GetFacetDefinitions();
            var facetGroups = facets
                .Where(x => !x.IsCommerceEnabled)
                .Select(x => new FacetGroupOption
                {
                    GroupFieldName = x.FieldName,
                    GroupName = x.DisplayName,
                }).ToList();

            query = facets.Aggregate(query, (current, facet) => facet.Facet(current, GetSelectedFilter(options, facet.FieldName)));

            var contentFacetsResult = query.Take(0).GetResult();
            if (contentFacetsResult.Facets == null)
            {
                return facetGroups;
            }

            foreach (var facetGroup in facetGroups)
            {
                var filter = facets.FirstOrDefault(x => x.FieldName.Equals(facetGroup.GroupFieldName));
                if (filter == null)
                {
                    continue;
                }

                var facet = contentFacetsResult.Facets.FirstOrDefault(x => x.Name.Equals(facetGroup.GroupFieldName));
                if (facet == null)
                {
                    continue;
                }

                filter.PopulateFacet(facetGroup, facet, selectedfacets);
            }
            return facetGroups;
        }

        private Filter GetSelectedFilter(List<FacetGroupOption> options, string currentField)
        {
            var filters = new List<Filter>();
            var facets = _facetRegistry.GetFacetDefinitions();
            foreach (var facetGroupOption in options)
            {
                if (facetGroupOption.GroupFieldName.Equals(currentField))
                {
                    continue;
                }

                var filter = facets.FirstOrDefault(x => x.FieldName.Equals(facetGroupOption.GroupFieldName));
                if (filter == null)
                {
                    continue;
                }

                if (!facetGroupOption.Facets.Any(x => x.Selected))
                {
                    continue;
                }

                if (filter is FacetStringDefinition)
                {
                    filters.Add(new TermsFilter(_findClient.GetFullFieldName(facetGroupOption.GroupFieldName, typeof(string)),
                        facetGroupOption.Facets.Where(x => x.Selected).Select(x => FieldFilterValue.Create(x.Name))));
                }
                else if (filter is FacetStringListDefinition)
                {
                    var termFilters = facetGroupOption.Facets.Where(x => x.Selected)
                        .Select(s => new TermFilter(facetGroupOption.GroupFieldName, FieldFilterValue.Create(s.Name)))
                        .Cast<Filter>()
                        .ToList();

                    filters.AddRange(termFilters);
                }
            }

            if (!filters.Any())
            {
                return null;
            }

            if (filters.Count == 1)
            {
                return filters.FirstOrDefault();
            }

            var boolFilter = new BoolFilter();
            foreach (var filter in filters)
            {
                boolFilter.Should.Add(filter);
            }
            return boolFilter;
        }
    }
}
