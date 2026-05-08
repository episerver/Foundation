// Phase 5: Graph search implementation.
// Graph is the primary search path; falls back to in-memory if Graph is unavailable or not yet indexed.
// Prerequisites: services.AddContentGraph() and services.AddGraphContentClient() in Startup.cs.
// Content must be synced to Optimizely Content Cloud before Graph queries return results.
using EPiServer.Globalization;
using EPiServer.Web.Routing;
using Foundation.Features.Blocks.ProductFilterBlocks;
using Foundation.Features.CatalogContent;
using Foundation.Features.CatalogContent.Package;
using Foundation.Features.CatalogContent.Product;
using Foundation.Features.CatalogContent.Services;
using Foundation.Features.MyOrganization.Users;
using Foundation.Features.MyOrganization.QuickOrderPage; // SkuSearchResultModel
using Foundation.Features.NewProducts;
using Foundation.Features.Sales;
using Foundation.Features.Search.Category;
using Foundation.Infrastructure.Find.Facets;
using Microsoft.Extensions.Logging;
using Optimizely.Graph.Cms.Query;
using Optimizely.Graph.Cms.Query.Implementation; // OrderDirection
using static Foundation.Features.Shared.SelectionFactories.InclusionOrderingSelectionFactory;

namespace Foundation.Features.Search
{
    public interface ISearchService
    {
        ProductSearchResults Search(IContent currentContent, FilterOptionViewModel filterOptions, string selectedFacets, int catalogId = 0, IEnumerable<Func<EntryContentBase, bool>> filters = null);
        // SearchWithFilters removed: only used by ProductSearchBlockComponent which is excluded (uses EPiServer.Find Filter types).
        IEnumerable<ProductTileViewModel> SearchOnSale(SalesPage currentContent, out List<int> pages, int catalogId = 0, int page = 1, int pageSize = 12);
        IEnumerable<ProductTileViewModel> SearchNewProducts(NewProductsPage currentContent, out List<int> pages, int catalogId = 0, int page = 1, int pageSize = 12);
        IEnumerable<ProductTileViewModel> QuickSearch(string query, int catalogId = 0);
        IEnumerable<ProductTileViewModel> QuickSearch(FilterOptionViewModel filterOptions, int catalogId = 0);
        IEnumerable<SortOrder> GetSortOrder();
        string GetOutline(string nodeCode);
        IEnumerable<UserSearchResultModel> SearchUsers(string query, int page = 1, int pageSize = 50);
        IEnumerable<SkuSearchResultModel> SearchSkus(string query);
        ContentSearchViewModel SearchContent(FilterOptionViewModel filterOptions);
        ContentSearchViewModel SearchPdf(FilterOptionViewModel filterOptions);
    }

    public class SearchService : ISearchService
    {
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyService;
        private readonly IContentLanguageAccessor _contentLanguageAccessor;
        private readonly ReferenceConverter _referenceConverter;
        private readonly IContentRepository _contentRepository;
        private readonly IPriceService _priceService;
        private readonly IPromotionService _promotionService;
        private readonly IContentLoader _contentLoader;
        private readonly IProductService _productService;
        private readonly UrlResolver _urlResolver;
        private readonly IGraphContentClient _graphClient;
        private readonly ILogger<SearchService> _logger;

        public SearchService(
            ICurrentMarket currentMarket,
            ICurrencyService currencyService,
            IContentLanguageAccessor contentLanguageAccessor,
            ReferenceConverter referenceConverter,
            IContentRepository contentRepository,
            IPriceService priceService,
            IPromotionService promotionService,
            IContentLoader contentLoader,
            IProductService productService,
            UrlResolver urlResolver,
            IGraphContentClient graphClient,
            ILogger<SearchService> logger)
        {
            _currentMarket = currentMarket;
            _currencyService = currencyService;
            _contentLanguageAccessor = contentLanguageAccessor;
            _referenceConverter = referenceConverter;
            _contentRepository = contentRepository;
            _priceService = priceService;
            _promotionService = promotionService;
            _contentLoader = contentLoader;
            _productService = productService;
            _urlResolver = urlResolver;
            _graphClient = graphClient;
            _logger = logger;
        }

        // ── Product search ──────────────────────────────────────────────────────────

        public ProductSearchResults Search(IContent currentContent, FilterOptionViewModel filterOptions, string selectedFacets, int catalogId = 0, IEnumerable<Func<EntryContentBase, bool>> filters = null)
        {
            var query = filterOptions?.Q ?? "";
            var page = filterOptions?.Page > 0 ? filterOptions.Page : 1;
            var pageSize = filterOptions?.PageSize > 0 ? filterOptions.PageSize : 12;
            var skip = (page - 1) * pageSize;

            // Without a text query we're in catalogue-browse mode; in-memory handles ancestor
            // scoping correctly and is fast enough for a reference catalogue.
            if (string.IsNullOrWhiteSpace(query))
                return SearchProductsInMemory(currentContent, filterOptions, filters);

            // Text query: use Graph for full-text search across all product content.
            // TODO: add catalogue-node scoping once ProductContent field mapping for
            // ParentLink / Ancestors is confirmed in the Graph schema.
            try
            {
                var result = _graphClient
                    .QueryContent<ProductContent>()
                    .SearchFor(query)
                    .UsingFullText()
                    .Skip(skip)
                    .Limit(pageSize)
                    .IncludeTotal()
                    .GetAsContentAsync()
                    .GetAwaiter().GetResult();

                // If Graph returned results, use them.
                // If Graph returned 0, it likely hasn't indexed Commerce products yet — fall through
                // to in-memory so the site stays functional while indexing is pending.
                if ((result.Total ?? 0) > 0)
                {
                    return new ProductSearchResults
                    {
                        ProductViewModels = _productService.GetProductTileViewModels(result.Select(e => e.ContentLink)),
                        FacetGroups = Enumerable.Empty<FacetGroupOption>(),
                        TotalCount = result.Total ?? 0,
                        Query = query
                    };
                }

                _logger.LogInformation("Graph product search returned 0 results for '{Query}'; falling back to in-memory.", query);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Graph product search failed; falling back to in-memory scan.");
            }

            return SearchProductsInMemory(currentContent, filterOptions, filters);
        }

        public IEnumerable<ProductTileViewModel> QuickSearch(string query, int catalogId = 0)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Enumerable.Empty<ProductTileViewModel>();

            try
            {
                var result = _graphClient
                    .QueryContent<ProductContent>()
                    .SearchFor(query)
                    .UsingFullText()
                    .Limit(6)
                    .GetAsContentAsync()
                    .GetAwaiter().GetResult();

                // Use Graph results when available; fall through to in-memory if Graph has 0
                // (indicates products not yet indexed in Content Cloud).
                if (result.Any())
                    return _productService.GetProductTileViewModels(result.Select(e => e.ContentLink));

                _logger.LogInformation("Graph quick search returned 0 results for '{Query}'; falling back to in-memory.", query);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Graph quick search failed; falling back to in-memory.");
            }

            var matches = GetCatalogEntries<ProductContent>(_referenceConverter.GetRootLink())
                .Where(e => e.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                         || e.Code.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Take(6)
                .ToList();
            return _productService.GetProductTileViewModels(matches.Select(e => e.ContentLink));
        }

        public IEnumerable<ProductTileViewModel> QuickSearch(FilterOptionViewModel filterOptions, int catalogId = 0)
            => QuickSearch(filterOptions?.Q ?? "", catalogId);

        public IEnumerable<ProductTileViewModel> SearchOnSale(SalesPage currentContent, out List<int> pages, int catalogId = 0, int page = 1, int pageSize = 12)
        {
            // Stub: detecting on-sale products requires price+promotion indexing in Graph; not yet implemented.
            pages = new List<int>();
            return Enumerable.Empty<ProductTileViewModel>();
        }

        public IEnumerable<ProductTileViewModel> SearchNewProducts(NewProductsPage currentContent, out List<int> pages, int catalogId = 0, int page = 1, int pageSize = 12)
        {
            var skip = (page - 1) * pageSize;
            try
            {
                var result = _graphClient
                    .QueryContent<ProductContent>()
                    .OrderBy(x => x.Created, OrderDirection.Descending)
                    .Skip(skip)
                    .Limit(pageSize)
                    .IncludeTotal()
                    .GetAsContentAsync()
                    .GetAwaiter().GetResult();

                if ((result.Total ?? 0) > 0)
                {
                    var totalCount = result.Total ?? 0;
                    pages = Enumerable.Range(1, Math.Max(1, (int)Math.Ceiling(totalCount / (double)pageSize))).ToList();
                    return _productService.GetProductTileViewModels(result.Select(p => p.ContentLink));
                }

                _logger.LogInformation("Graph new-products query returned 0 results; falling back to in-memory.");
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Graph new-products query failed; falling back to in-memory.");
            }

            var allProducts = GetCatalogEntries<ProductContent>(_referenceConverter.GetRootLink())
                .OrderByDescending(p => p.Created)
                .ToList();
            var count = allProducts.Count;
            pages = Enumerable.Range(1, Math.Max(1, (int)Math.Ceiling(count / (double)pageSize))).ToList();
            return _productService.GetProductTileViewModels(
                allProducts.Skip(skip).Take(pageSize).Select(p => p.ContentLink));
        }

        // ── SKU / user search ───────────────────────────────────────────────────────

        public IEnumerable<SkuSearchResultModel> SearchSkus(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return Enumerable.Empty<SkuSearchResultModel>();

            return GetCatalogEntries<VariationContent>(_referenceConverter.GetRootLink())
                .Where(v => v.Code.Contains(query, StringComparison.OrdinalIgnoreCase)
                         || v.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Take(10)
                .Select(v => new SkuSearchResultModel
                {
                    Sku = v.Code,
                    ProductName = v.Name,
                    UrlImage = string.Empty,
                    UnitPrice = 0m
                })
                .ToList();
        }

        public IEnumerable<UserSearchResultModel> SearchUsers(string query, int page = 1, int pageSize = 50)
            => Enumerable.Empty<UserSearchResultModel>();

        // ── Content / PDF search ────────────────────────────────────────────────────

        public ContentSearchViewModel SearchContent(FilterOptionViewModel filterOptions)
        {
            var query = filterOptions?.Q;
            if (string.IsNullOrWhiteSpace(query))
                return new ContentSearchViewModel { FilterOption = filterOptions, Hits = Enumerable.Empty<UnifiedSearchHit>() };

            try
            {
                var result = _graphClient
                    .QueryContent<PageData>()
                    .SearchFor(query)
                    .UsingFullText()
                    .Limit(20)
                    .GetAsContentAsync()
                    .GetAwaiter().GetResult();

                if (result.Any())
                {
                    return new ContentSearchViewModel
                    {
                        FilterOption = filterOptions,
                        Hits = result.Select(p => new UnifiedSearchHit
                        {
                            Title = p.Name,
                            Url = _urlResolver.GetUrl(p.ContentLink),
                            Excerpt = string.Empty,
                            SearchSection = "Pages"
                        })
                    };
                }

                _logger.LogInformation("Graph content search returned 0 results for '{Query}'; falling back to in-memory.", query);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Graph content search failed; falling back to in-memory.");
            }

            return SearchContentInMemory(filterOptions);
        }

        public ContentSearchViewModel SearchPdf(FilterOptionViewModel filterOptions)
        {
            var query = filterOptions?.Q;
            if (string.IsNullOrWhiteSpace(query))
                return new ContentSearchViewModel { FilterOption = filterOptions, Hits = Enumerable.Empty<UnifiedSearchHit>() };

            try
            {
                // Fetch extra results and post-filter to PDFs — Graph does not natively filter
                // by file extension; a MimeType filter can be added once schema mapping is confirmed.
                var result = _graphClient
                    .QueryContent<MediaData>()
                    .SearchFor(query)
                    .UsingFullText()
                    .Limit(50)
                    .GetAsContentAsync()
                    .GetAwaiter().GetResult();

                if (result.Any())
                {
                    return new ContentSearchViewModel
                    {
                        FilterOption = filterOptions,
                        Hits = result
                            .Where(m => m.Name.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                            .Take(10)
                            .Select(m => new UnifiedSearchHit
                            {
                                Title = m.Name,
                                Url = _urlResolver.GetUrl(m.ContentLink),
                                Excerpt = string.Empty,
                                SearchSection = "PDF"
                            })
                    };
                }

                _logger.LogInformation("Graph PDF search returned 0 results for '{Query}'; falling back to in-memory.", query);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Graph PDF search failed; falling back to in-memory.");
            }

            return SearchPdfInMemory(filterOptions);
        }

        // ── Sort / outline helpers ──────────────────────────────────────────────────

        public IEnumerable<SortOrder> GetSortOrder()
        {
            return new List<SortOrder>
            {
                new SortOrder { Name = ProductSortOrder.Popularity, Key = "", SortDirection = SortDirection.Ascending },
                new SortOrder { Name = ProductSortOrder.NewestFirst, Key = "created", SortDirection = SortDirection.Descending }
            };
        }

        public string GetOutline(string nodeCode) => GetOutlineForNode(nodeCode);

        // ── Private helpers: in-memory fallbacks ────────────────────────────────────

        private ProductSearchResults SearchProductsInMemory(IContent currentContent, FilterOptionViewModel filterOptions, IEnumerable<Func<EntryContentBase, bool>> filters)
        {
            var query = filterOptions?.Q ?? "";
            var page = filterOptions?.Page > 0 ? filterOptions.Page : 1;
            var pageSize = filterOptions?.PageSize > 0 ? filterOptions.PageSize : 12;

            var rootLink = currentContent is CatalogContentBase
                ? currentContent.ContentLink
                : _referenceConverter.GetRootLink();

            var allEntries = GetCatalogEntries<ProductContent>(rootLink);

            if (!string.IsNullOrWhiteSpace(query))
                allEntries = allEntries
                    .Where(e => e.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                             || e.Code.Contains(query, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            if (filters != null)
                foreach (var predicate in filters)
                    allEntries = allEntries.Where(e => predicate(e)).ToList();

            var totalCount = allEntries.Count;
            var paged = allEntries.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return new ProductSearchResults
            {
                ProductViewModels = _productService.GetProductTileViewModels(paged.Select(e => e.ContentLink)),
                FacetGroups = Enumerable.Empty<FacetGroupOption>(),
                TotalCount = totalCount,
                Query = query
            };
        }

        private ContentSearchViewModel SearchContentInMemory(FilterOptionViewModel filterOptions)
        {
            var query = filterOptions.Q;
            var hits = _contentLoader.GetDescendents(ContentReference.RootPage)
                .Select(r => { try { return _contentLoader.Get<IContent>(r); } catch { return null; } })
                .OfType<PageData>()
                .Where(p => !p.IsDeleted && p.Name.Contains(query, StringComparison.OrdinalIgnoreCase))
                .Take(20)
                .Select(p => new UnifiedSearchHit
                {
                    Title = p.Name,
                    Url = _urlResolver.GetUrl(p.ContentLink),
                    Excerpt = string.Empty,
                    SearchSection = "Pages"
                });
            return new ContentSearchViewModel { FilterOption = filterOptions, Hits = hits };
        }

        private ContentSearchViewModel SearchPdfInMemory(FilterOptionViewModel filterOptions)
        {
            var query = filterOptions.Q;
            var hits = _contentLoader.GetDescendents(ContentReference.RootPage)
                .Select(r => { try { return _contentLoader.Get<IContent>(r); } catch { return null; } })
                .OfType<MediaData>()
                .Where(m => m.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                         && m.Name.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase))
                .Take(10)
                .Select(m => new UnifiedSearchHit
                {
                    Title = m.Name,
                    Url = _urlResolver.GetUrl(m.ContentLink),
                    Excerpt = string.Empty,
                    SearchSection = "PDF"
                });
            return new ContentSearchViewModel { FilterOption = filterOptions, Hits = hits };
        }

        /// <summary>
        /// Returns all catalog descendants of <paramref name="rootLink"/> that are of type <typeparamref name="T"/>.
        /// Uses Get&lt;EntryContentBase&gt; (not Get&lt;IContent&gt;) — Commerce 15 content provider
        /// requires an entry-specific type; Get&lt;IContent&gt; may return a non-castable proxy.
        /// </summary>
        /// <remarks>
        /// In Commerce 15, <c>GetDescendents</c> on the catalog system root (from
        /// <c>ReferenceConverter.GetRootLink()</c>) returns nothing — it only works on
        /// <c>CatalogContent</c> or <c>NodeContent</c> links.  When the root reference is
        /// detected, this method enumerates each top-level catalog's descendants instead.
        /// </remarks>
        private List<T> GetCatalogEntries<T>(ContentReference rootLink) where T : class, IContent
        {
            IEnumerable<ContentReference> refs;
            var catalogRoot = _referenceConverter.GetRootLink();

            if (!ContentReference.IsNullOrEmpty(rootLink) && rootLink == catalogRoot)
            {
                // Catalog root: GetDescendents returns nothing in Commerce 15.
                // Walk each top-level CatalogContent and collect their descendants.
                refs = _contentLoader
                    .GetChildren<EPiServer.Commerce.Catalog.ContentTypes.CatalogContent>(catalogRoot)
                    .SelectMany(c => _contentLoader.GetDescendents(c.ContentLink));
            }
            else
            {
                refs = _contentLoader.GetDescendents(rootLink);
            }

            return refs
                .Select(r =>
                {
                    try { return _contentLoader.Get<EntryContentBase>(r) as T; }
                    catch { return null; }
                })
                .Where(e => e != null && !e.IsDeleted)
                .ToList();
        }

        private string GetOutlineForNode(string nodeCode)
        {
            if (string.IsNullOrEmpty(nodeCode))
                return "";

            var outline = nodeCode;
            var currentNode = _contentRepository.Get<NodeContent>(_referenceConverter.GetContentLink(nodeCode));
            var parent = _contentRepository.Get<CatalogContentBase>(currentNode.ParentLink);
            while (!ContentReference.IsNullOrEmpty(parent.ParentLink))
            {
                if (parent is EPiServer.Commerce.Catalog.ContentTypes.CatalogContent catalog)
                    outline = $"{catalog.Name}/{outline}";
                else if (parent is NodeContent parentNode)
                    outline = $"{parentNode.Code}/{outline}";

                parent = _contentRepository.Get<CatalogContentBase>(parent.ParentLink);
            }
            return outline;
        }
    }
}
