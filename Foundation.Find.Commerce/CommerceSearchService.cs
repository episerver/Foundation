using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Api.Querying.Filters;
using EPiServer.Find.Cms;
using EPiServer.Find.Commerce;
using EPiServer.Find.Framework.BestBets;
using EPiServer.Find.Framework.Statistics;
using EPiServer.Find.Statistics;
using EPiServer.Find.UI;
using EPiServer.Globalization;
using EPiServer.Security;
using Foundation.Commerce.Catalog;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Markets;
using Foundation.Commerce.Models.Catalog;
using Foundation.Commerce.Models.Pages;
using Foundation.Find.Cms;
using Foundation.Find.Cms.Facets;
using Foundation.Find.Commerce.ViewModels;
using Mediachase.Commerce;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Pricing;
using Mediachase.Commerce.Security;
using Mediachase.Commerce.Website.Search;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Helpers;

namespace Foundation.Find.Commerce
{
    public class CommerceSearchService : ICommerceSearchService
    {
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyService;
        private readonly LanguageResolver _languageResolver;
        private readonly IClient _findClient;
        private readonly IFacetRegistry _facetRegistry;
        private const int DefaultPageSize = 18;
        private readonly IFindUIConfiguration _findUIConfiguration;
        private readonly ReferenceConverter _referenceConverter;
        private readonly IContentRepository _contentRepository;
        private readonly IPriceService _priceService;
        private readonly IPromotionService _promotionService;
        private readonly ICurrencyService _currencyservice;

        public CommerceSearchService(ICurrentMarket currentMarket,
            ICurrencyService currencyService,
            LanguageResolver languageResolver,
            IClient findClient,
            IFacetRegistry facetRegistry,
            IFindUIConfiguration findUIConfiguration,
            ReferenceConverter referenceConverter,
            IContentRepository contentRepository,
            IPriceService priceService,
            IPromotionService promotionService,
            ICurrencyService currencyservice
            )
        {
            _currentMarket = currentMarket;
            _currencyService = currencyService;
            _languageResolver = languageResolver;
            _findClient = findClient;
            _facetRegistry = facetRegistry;
            _findUIConfiguration = findUIConfiguration;
            //_findClient.Personalization().Refresh();
            _referenceConverter = referenceConverter;
            _contentRepository = contentRepository;
            _priceService = priceService;
            _promotionService = promotionService;
            _currencyservice = currencyservice;
        }

        public ProductSearchResults Search(IContent currentContent,
            CommerceFilterOptionViewModel filterOptions,
            string selectedFacets,
            int catalogId = 0) => filterOptions == null ? CreateEmptyResult() : GetSearchResults(currentContent, filterOptions, selectedFacets, null, catalogId);

        public ProductSearchResults SearchWithFilters(IContent currentContent,
            CommerceFilterOptionViewModel filterOptions,
            IEnumerable<Filter> filters,
            int catalogId = 0) => filterOptions == null ? CreateEmptyResult() : GetSearchResults(currentContent, filterOptions, "", filters, catalogId);

        public IEnumerable<ProductTileViewModel> SearchOnSale(IContent currentContent)
        {
            var market = _currentMarket.GetCurrentMarket();
            var query = _findClient.Search<EntryContentBase>();
            query = query.FilterMarket(market);
            query = query.Filter(x => x.Language.Name.Match(_languageResolver.GetPreferredCulture().Name));
            query = query.FilterForVisitor();
            query = query.Filter(x => (x as GenericProduct).OnSale.Match(true));
            var result = query.GetContentResult();
            return CreateProductViewModels(result, currentContent, "");
        }

        public IEnumerable<ProductTileViewModel> SearchNewProducts(IContent currentContent)
        {
            var market = _currentMarket.GetCurrentMarket();
            var query = _findClient.Search<EntryContentBase>();
            query = query.FilterMarket(market);
            query = query.Filter(x => x.Language.Name.Match(_languageResolver.GetPreferredCulture().Name));
            query = query.FilterForVisitor();
            query = query.OrderByDescending(x => x.Created);
            query = query.Skip(0)
                        .Take((currentContent as NewProductsPage).NumberOfProducts)
                        .StaticallyCacheFor(TimeSpan.FromMinutes(1));
            var result = query.GetContentResult();
            return CreateProductViewModels(result, currentContent, "");
        }

        public IEnumerable<ProductTileViewModel> QuickSearch(string query)
        {
            var filterOptions = new CommerceFilterOptionViewModel
            {
                Q = query,
                PageSize = 5,
                Sort = string.Empty,
                FacetGroups = new List<FacetGroupOption>(),
                Page = 1,
                TrackData = false
            };
            return QuickSearch(filterOptions);
        }


        public IEnumerable<ProductTileViewModel> QuickSearch(CommerceFilterOptionViewModel filterOptions) => string.IsNullOrEmpty(filterOptions.Q) ? Enumerable.Empty<ProductTileViewModel>() : GetSearchResults(null, filterOptions, "").ProductViewModels;

        public IEnumerable<SortOrder> GetSortOrder()
        {
            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyService.GetCurrentCurrency();

            return new List<SortOrder>
            {
                //new SortOrder {Name = ProductSortOrder.PriceAsc, Key = IndexingHelper.GetPriceField(market.MarketId, currency), SortDirection = SortDirection.Ascending},
                new SortOrder {Name = ProductSortOrder.Popularity, Key = "", SortDirection = SortDirection.Ascending},
                new SortOrder {Name = ProductSortOrder.NewestFirst, Key = "created", SortDirection = SortDirection.Descending}
            };
        }

        private ProductSearchResults GetSearchResults(IContent currentContent,
            CommerceFilterOptionViewModel filterOptions,
            string selectedfacets,
            IEnumerable<Filter> filters = null,
            int catalogId = 0)
        {

            //If contact belong organization, only find product that belong the categories that has owner is this organization
            var contact = PrincipalInfo.CurrentPrincipal.GetCustomerContact();
            var organizationId = contact?.ContactOrganization?.PrimaryKeyId ?? Guid.Empty;
            CatalogContent catalogOrganization = null;
            if (organizationId != Guid.Empty)
            {
                //get category that has owner id = organizationId
                catalogOrganization = _contentRepository
                    .GetChildren<CatalogContent>(_referenceConverter.GetRootLink())
                    .FirstOrDefault(x => !string.IsNullOrEmpty(x.Owner) && x.Owner.Equals(organizationId.ToString(), StringComparison.OrdinalIgnoreCase));
            }

            var pageSize = filterOptions.PageSize > 0 ? filterOptions.PageSize : DefaultPageSize;
            var market = _currentMarket.GetCurrentMarket();

            var query = _findClient.Search<EntryContentBase>();
            query = ApplyTermFilter(query, filterOptions.Q, filterOptions.TrackData);
            query = query.Filter(x => x.Language.Name.Match(_languageResolver.GetPreferredCulture().Name));

            if (organizationId != Guid.Empty && catalogOrganization != null)
            {
                query = query.Filter(x => x.Outline().PrefixCaseInsensitive(catalogOrganization.Name));
            }

            var nodeContent = currentContent as NodeContent;
            if (nodeContent != null)
            {
                var outline = GetOutline(nodeContent.Code);
                query = query.FilterOutline(new[] { outline });
            }

            query = query.FilterMarket(market);
            var facetQuery = query;

            query = FilterSelected(query, filterOptions.FacetGroups);
            query = ApplyFilters(query, filters);
            query = OrderBy(query, filterOptions);
            //Exclude products from search
            //query = query.Filter(x => (x as ProductContent).ExcludeFromSearch.Match(false));

            if (catalogId != 0)
            {
                query = query.Filter(x => x.CatalogId.Match(catalogId));
            }

            query = query.ApplyBestBets()
                .Skip((filterOptions.Page - 1) * pageSize)
                .Take(pageSize)
                .StaticallyCacheFor(TimeSpan.FromMinutes(1));

            var result = query.GetContentResult();

            return new ProductSearchResults
            {
                ProductViewModels = CreateProductViewModels(result, currentContent, filterOptions.Q),
                FacetGroups = GetFacetResults(filterOptions.FacetGroups, facetQuery, selectedfacets),
                TotalCount = result.TotalMatching,
                DidYouMeans = string.IsNullOrEmpty(filterOptions.Q) ? null : _findClient.Statistics().GetDidYouMean(filterOptions.Q),
                Query = filterOptions.Q,
            };

        }



        public IEnumerable<ProductTileViewModel> CreateProductViewModels(IContentResult<EntryContentBase> searchResult, IContent content, string searchQuery)
        {
            List<ProductTileViewModel> productViewModels = null;
            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyService.GetCurrentCurrency();

            if (searchResult == null)
            {
                throw new ArgumentNullException(nameof(searchResult));
            }

            productViewModels = searchResult.Select(document => document.GetProductTileViewModel(market, currency)).ToList();
            ApplyBoostedProperties(ref productViewModels, searchResult, content, searchQuery);
            return productViewModels;
        }

        public virtual StringCollection GetOutlinesForNode(string code)
        {
            var nodes = SearchFilterHelper.GetOutlinesForNode(code);
            if (nodes.Count == 0)
            {
                return nodes;
            }
            nodes[nodes.Count - 1] = nodes[nodes.Count - 1].Replace("*", "");
            return nodes;
        }

        public virtual string GetOutline(string nodeCode) => GetOutlineForNode(nodeCode);

        private string GetOutlineForNode(string nodeCode)
        {
            if (string.IsNullOrEmpty(nodeCode))
            {
                return "";
            }
            var outline = nodeCode;
            var currentNode = _contentRepository.Get<NodeContent>(_referenceConverter.GetContentLink(nodeCode));
            var parent = _contentRepository.Get<CatalogContentBase>(currentNode.ParentLink);
            while (!ContentReference.IsNullOrEmpty(parent.ParentLink))
            {
                var catalog = parent as CatalogContent;
                if (catalog != null)
                {
                    outline = string.Format("{1}/{0}", outline, catalog.Name);
                }

                var parentNode = parent as NodeContent;
                if (parentNode != null)
                {
                    outline = string.Format("{1}/{0}", outline, parentNode.Code);
                }

                parent = _contentRepository.Get<CatalogContentBase>(parent.ParentLink);

            }
            return outline;
        }

        private static ITypeSearch<EntryContentBase> ApplyTermFilter(ITypeSearch<EntryContentBase> query, string searchTerm, bool trackData)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return query;
            }

            query = query.For(searchTerm);
            if (trackData)
            {
                query = query.Track();
            }

            return query;
        }

        private ITypeSearch<EntryContentBase> OrderBy(ITypeSearch<EntryContentBase> query, CommerceFilterOptionViewModel CommerceFilterOptionViewModel)
        {
            if (string.IsNullOrEmpty(CommerceFilterOptionViewModel.Sort) || CommerceFilterOptionViewModel.Sort.Equals("Position"))
            {
                if (CommerceFilterOptionViewModel.SortDirection.Equals("Asc"))
                {
                    query = query.OrderBy(x => x.SortOrder());
                    return query;
                }
                query = query.OrderByDescending(x => x.SortOrder());
                return query;
            }

            if (CommerceFilterOptionViewModel.Sort.Equals("Price"))
            {
                if (CommerceFilterOptionViewModel.SortDirection.Equals("Asc"))
                {
                    query = query.OrderBy(x => x.DefaultPrice());
                    return query;
                }
                query = query.OrderByDescending(x => x.DefaultPrice());
                return query;
            }

            if (CommerceFilterOptionViewModel.Sort.Equals("Name"))
            {
                if (CommerceFilterOptionViewModel.SortDirection.Equals("Asc"))
                {
                    query = query.OrderBy(x => x.DisplayName);
                    return query;
                }
                query = query.OrderByDescending(x => x.DisplayName);
                return query;
            }

            //if (CommerceFilterOptionViewModel.Sort.Equals("Recommended"))
            //{
            //    query = query.UsingPersonalization();
            //    return query;
            //}

            return query;
        }

        private IEnumerable<FacetGroupOption> GetFacetResults(List<FacetGroupOption> options,
            ITypeSearch<EntryContentBase> query,
            string selectedfacets)
        {
            if (options == null)
            {
                return Enumerable.Empty<FacetGroupOption>();
            }

            var facets = _facetRegistry.GetFacetDefinitions();
            var facetGroups = facets.Select(x => new FacetGroupOption
            {
                GroupFieldName = x.FieldName,
                GroupName = x.DisplayName,

            }).ToList();

            query = facets.Aggregate(query, (current, facet) => facet.Facet(current, GetSelectedFilter(options, facet.FieldName)));

            var productFacetsResult = query.Take(0).GetContentResult();
            if (productFacetsResult.Facets == null)
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

                var facet = productFacetsResult.Facets.FirstOrDefault(x => x.Name.Equals(facetGroup.GroupFieldName));
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
                else if (filter is FacetNumericRangeDefinition)
                {
                    var rangeFilters = filter as FacetNumericRangeDefinition;
                    foreach (var selectedRange in facetGroupOption.Facets.Where(x => x.Selected))
                    {
                        var rangeFilter = rangeFilters.Range.FirstOrDefault(x => x.Id.Equals(selectedRange.Key.Split(':')[1]));
                        if (rangeFilter == null)
                        {
                            continue;
                        }
                        filters.Add(RangeFilter.Create(_findClient.GetFullFieldName(facetGroupOption.GroupFieldName, typeof(double)),
                            rangeFilter.From ?? 0,
                            rangeFilter.To ?? double.MaxValue));
                    }
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

        private ITypeSearch<T> FilterSelected<T>(ITypeSearch<T> query, List<FacetGroupOption> options)
        {
            var facets = _facetRegistry.GetFacetDefinitions();

            foreach (var facetGroupOption in options)
            {
                var filter = facets.FirstOrDefault(x => x.FieldName.Equals(facetGroupOption.GroupFieldName));
                if (filter == null)
                {
                    continue;
                }

                if (facetGroupOption.Facets != null && !facetGroupOption.Facets.Any(x => x.Selected))
                {
                    continue;
                }

                if (filter is FacetStringDefinition)
                {
                    var stringFilter = filter as FacetStringDefinition;
                    query = stringFilter.Filter(query, facetGroupOption.Facets
                        .Where(x => x.Selected)
                        .Select(x => x.Name).ToList());
                }
                else if (filter is FacetStringListDefinition)
                {
                    var stringListFilter = filter as FacetStringListDefinition;
                    query = stringListFilter.Filter(query, facetGroupOption.Facets
                        .Where(x => x.Selected)
                        .Select(x => x.Name).ToList());
                }
                else if (filter is FacetNumericRangeDefinition)
                {
                    var numericFilter = filter as FacetNumericRangeDefinition;
                    var ranges = new List<SelectableNumericRange>();
                    var selectedFacets = facetGroupOption.Facets.Where(x => x.Selected);
                    foreach (var facetOption in selectedFacets)
                    {
                        var range = numericFilter.Range.FirstOrDefault(x => x.Id.Equals(facetOption.Key.Split(':')[1]));
                        if (range == null)
                        {
                            continue;
                        }
                        ranges.Add(new SelectableNumericRange
                        {
                            From = range.From,
                            Id = range.Id,
                            Selected = range.Selected,
                            To = range.To
                        });
                    }

                    query = numericFilter.Filter(query, ranges);
                }
            }
            return query;
        }

        private ITypeSearch<EntryContentBase> ApplyFilters(ITypeSearch<EntryContentBase> query,
            IEnumerable<Filter> filters)
        {
            if (filters == null || !filters.Any())
            {
                return query;
            }

            foreach (var filter in filters)
            {
                query = query.Filter(filter);
            }
            return query;

        }

        private static ProductSearchResults CreateEmptyResult()
        {
            return new ProductSearchResults
            {
                ProductViewModels = Enumerable.Empty<ProductTileViewModel>(),
                FacetGroups = Enumerable.Empty<FacetGroupOption>(),
            };
        }

        /// <summary>
        /// Sets Featured Product property and Best Bet Product property to ProductViewModels.
        /// </summary>
        /// <param name="searchResult">The search result (product list).</param>
        /// <param name="currentContent">The product category.</param>
        /// <param name="searchQuery">The search query string to filter Best Bet result.</param>
        /// <param name="productViewModels">The ProductViewModels is added two properties: Featured Product and Best Bet.</param>
        private void ApplyBoostedProperties(ref List<ProductTileViewModel> productViewModels, IContentResult<EntryContentBase> searchResult, IContent currentContent, string searchQuery)
        {
            var node = currentContent as GenericNode;
            var products = new List<EntryContentBase>();

            if (node != null)
            {
                //products = node.FeaturedProducts?.FilteredItems?.Select(x => x.GetContent() as EntryContentBase).ToList() ?? new List<EntryContentBase>();

                var featuredProductList = productViewModels.Where(v => products.Any(p => p.ContentLink.ID == v.ProductId)).ToList();
                featuredProductList.ForEach(x => { x.IsFeaturedProduct = true; });

                productViewModels.RemoveAll(v => products.Any(p => p.ContentLink.ID == v.ProductId));
                productViewModels.InsertRange(0, featuredProductList);
            }

            var bestBetList = new BestBetRepository().List().Where(i => i.PhraseCriterion.Phrase.CompareTo(searchQuery) == 0);
            //Filter for product best bet only.
            var productBestBet = bestBetList.Where(i => i.BestBetSelector is CommerceBestBetSelector);
            productViewModels.ToList()
                             .ForEach(p =>
                             {
                                 if (productBestBet.Any(i => ((CommerceBestBetSelector)i.BestBetSelector).ContentLink.ID == p.ProductId))
                                 {
                                     p.IsBestBetProduct = true;
                                 }
                             });
        }

        public IEnumerable<UserSearchResultModel> SearchUsers(string query, int page = 1, int pageSize = 50)
        {
            var searchQuery = _findClient.Search<UserSearchResultModel>();
            if (!String.IsNullOrEmpty(query))
            {
                searchQuery = searchQuery.For(query);
            }
            var results = searchQuery.Skip((page - 1) * pageSize).Take(pageSize).GetResult();
            if (results != null && results.Any())
                return results.Hits.AsEnumerable().Select(x => x.Document);
            return Enumerable.Empty<UserSearchResultModel>();
        }

        public IEnumerable<SkuSearchResultModel> SearchSkus(string query)
        {
            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyservice.GetCurrentCurrency();

            var searchQuery = _findClient.Search<VariationContent>();
            searchQuery = searchQuery.Filter(x => x.Code.PrefixCaseInsensitive(query));
            searchQuery = searchQuery.FilterMarket(market);
            searchQuery = searchQuery.Filter(x => x.Language.Name.Match(_languageResolver.GetPreferredCulture().Name));
            searchQuery = searchQuery.Track();
            searchQuery = searchQuery.FilterForVisitor();
            var searchResults = searchQuery.GetContentResult();

            if (searchResults != null && searchResults.Any())
            {
                var searchResult = searchResults.Items;
                return searchResult.Select(variation =>
                {
                    var defaultPrice = _priceService.GetDefaultPrice(market.MarketId, DateTime.Now,
                        new CatalogKey(variation.Code), currency);
                    var discountedPrice = defaultPrice != null ? _promotionService.GetDiscountPrice(defaultPrice.CatalogKey, market.MarketId,
                        currency) : null;
                    return new SkuSearchResultModel
                    {
                        Sku = variation.Code,
                        ProductName = variation.DisplayName,
                        UnitPrice = discountedPrice?.UnitPrice.Amount ?? 0
                    };
                });
            }
            return Enumerable.Empty<SkuSearchResultModel>();
        }
    }
}
