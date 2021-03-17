using EPiServer;
using EPiServer.Core;
using EPiServer.Tracking.PageView;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using Foundation.Cms.Settings;
using Foundation.Features.CatalogContent;
using Foundation.Features.Home;
using Foundation.Features.Search.Search;
using Foundation.Features.Settings;
using Foundation.Personalization;
using Mediachase.Commerce.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Foundation.Features.Search
{
    public class SearchController : PageController<SearchResultPage>
    {
        private readonly ISearchViewModelFactory _viewModelFactory;
        private readonly ISearchService _searchService;
        private readonly ICommerceTrackingService _recommendationService;
        private readonly ReferenceConverter _referenceConverter;
        private readonly ICmsTrackingService _cmsTrackingService;
        private readonly HttpContextBase _httpContextBase;
        private readonly IContentLoader _contentLoader;
        private readonly ISettingsService _settingsService;

        public SearchController(
            ISearchViewModelFactory viewModelFactory,
            ISearchService searchService,
            ICommerceTrackingService recommendationService,
            ReferenceConverter referenceConverter,
            HttpContextBase httpContextBase,
            IContentLoader contentLoader,
            ICmsTrackingService cmsTrackingService,
            ISettingsService settingsService)
        {
            _viewModelFactory = viewModelFactory;
            _searchService = searchService;
            _recommendationService = recommendationService;
            _referenceConverter = referenceConverter;
            _cmsTrackingService = cmsTrackingService;
            _httpContextBase = httpContextBase;
            _contentLoader = contentLoader;
            _settingsService = settingsService;
        }

        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        [PageViewTracking]
        public async Task<ActionResult> Index(SearchResultPage currentPage, FilterOptionViewModel filterOptions)
        {
            if (filterOptions == null)
            {
                return Redirect(Url.ContentUrl(ContentReference.StartPage));
            }

            if (string.IsNullOrEmpty(filterOptions.ViewSwitcher))
            {
                filterOptions.ViewSwitcher = "Grid";
            }

            var searchSettings = _settingsService.GetSiteSettings<SearchSettings>();
            var startPage = _contentLoader.Get<HomePage>(ContentReference.StartPage);
            var viewModel = _viewModelFactory.Create(currentPage,
                HttpContext.Request.QueryString["facets"],
                searchSettings?.SearchCatalog ?? 0,
                filterOptions);

            if (viewModel == null)
            {
                return View(viewModel);
            }

            if (!searchSettings?.ShowProductSearchResults ?? false)
            {
                viewModel.ProductViewModels = new List<ProductTileViewModel>();
            }
            else
            {
                var bestBestList = viewModel.ProductViewModels.Where(x => x.IsBestBetProduct);
                var notBestBestList = viewModel.ProductViewModels.Where(x => !x.IsBestBetProduct);
                viewModel.ProductViewModels = bestBestList.Union(notBestBestList);

                if (filterOptions.Page <= 1 && HttpContext.Request.HttpMethod == "GET")
                {
                    var trackingResult =
                        await _recommendationService.TrackSearch(HttpContext, filterOptions.Q, filterOptions.PageSize,
                            viewModel.ProductViewModels.Select(x => x.Code));
                    viewModel.Recommendations = trackingResult.GetSearchResultRecommendations(_referenceConverter);
                }
            }

            await _cmsTrackingService.SearchedKeyword(_httpContextBase, filterOptions.Q);
            if (searchSettings?.ShowContentSearchResults ?? true)
            {
                viewModel.ContentSearchResult = _searchService.SearchContent(new FilterOptionViewModel()
                {
                    Q = filterOptions.Q,
                    PageSize = 5,
                    Page = filterOptions.SearchContent ? filterOptions.Page : 1,
                    SectionFilter = filterOptions.SectionFilter,
                    IncludeImagesContent = searchSettings?.IncludeImagesInContentsSearchResults ?? true
                });
            }

            if (searchSettings?.ShowPdfSearchResults ?? true)
            {
                viewModel.PdfSearchResult = _searchService.SearchPdf(new FilterOptionViewModel()
                {
                    Q = filterOptions.Q,
                    PageSize = 5,
                    Page = filterOptions.SearchPdf ? filterOptions.Page : 1,
                    SectionFilter = filterOptions.SectionFilter
                });
            }

            var productCount = viewModel.ProductViewModels?.Count() ?? 0;
            var contentCount = viewModel.ContentSearchResult?.Hits?.Count() ?? 0;
            var pdfCount = viewModel.PdfSearchResult?.Hits?.Count() ?? 0;

            if (productCount + contentCount + pdfCount == 1)
            {
                if (productCount == 1)
                {
                    var product = viewModel.ProductViewModels.FirstOrDefault();
                    return Redirect(product.Url);
                }
                if (contentCount == 1)
                {
                    var content = viewModel.ContentSearchResult.Hits.FirstOrDefault();
                    return Redirect(content.Url);
                }
                if (pdfCount == 1)
                {
                    var content = viewModel.PdfSearchResult.Hits.FirstOrDefault();
                    return Redirect(content.Url);
                }
            }

            viewModel.ShowProductSearchResults = searchSettings?.ShowProductSearchResults ?? true;
            viewModel.ShowContentSearchResults = searchSettings?.ShowContentSearchResults ?? true;
            viewModel.ShowPdfSearchResults = searchSettings?.ShowPdfSearchResults ?? true;

            return View(viewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult QuickSearch(string search = "")
        {
            var redirectUrl = "";
            var startPage = _contentLoader.Get<HomePage>(ContentReference.StartPage);
            var productCount = 0;
            var contentCount = 0;
            var pdfCount = 0;

            var model = new SearchViewModel<SearchResultPage>();
            var searchSettings = _settingsService.GetSiteSettings<SearchSettings>();
            if (searchSettings?.ShowProductSearchResults ?? true)
            {
                var productResults = _searchService.QuickSearch(search, searchSettings?.SearchCatalog ?? 0);
                model.ProductViewModels = productResults;
                productCount = productResults?.Count() ?? 0;

                // Push product search images over HTTP/2 if browser supports it
                if (productCount > 0)
                {
                    var links = new List<AssetPreloadLink>();

                    foreach (var productResult in model.ProductViewModels)
                    {
                        links.Add(new AssetPreloadLink(AssetPreloadLink.AssetType.Image) { NoPush = false, Url = productResult.ImageUrl + "?width=60" });
                    }

                    HttpContext.Response.AppendHeader("Link", string.Join(",", links));
                }
            }

            if (searchSettings?.ShowContentSearchResults ?? true)
            {
                var contentResult = _searchService.SearchContent(new FilterOptionViewModel()
                {
                    Q = search,
                    PageSize = 5,
                    Page = 1,
                    IncludeImagesContent = searchSettings?.IncludeImagesInContentsSearchResults ?? true
                });
                model.ContentSearchResult = contentResult;
                contentCount = contentResult?.Hits.Count() ?? 0;
            }

            if (searchSettings?.ShowPdfSearchResults ?? true)
            {
                var pdfResult = _searchService.SearchPdf(new FilterOptionViewModel()
                {
                    Q = search,
                    PageSize = 5,
                    Page = 1
                });
                model.PdfSearchResult = pdfResult;
                pdfCount = pdfResult?.Hits.Count() ?? 0;
            }

            if (productCount + contentCount + pdfCount == 1)
            {
                if (productCount == 1)
                {
                    var product = model.ProductViewModels.FirstOrDefault();
                    redirectUrl = product.Url;
                }
                if (contentCount == 1)
                {
                    var content = model.ContentSearchResult.Hits.FirstOrDefault();
                    redirectUrl = content.Url;
                }
                if (pdfCount == 1)
                {
                    var pdf = model.PdfSearchResult.Hits.FirstOrDefault();
                    redirectUrl = pdf.Url;
                }
            }
            model.RedirectUrl = redirectUrl;

            model.ShowProductSearchResults = searchSettings?.ShowProductSearchResults ?? true;
            model.ShowContentSearchResults = searchSettings?.ShowContentSearchResults ?? true;
            model.ShowPdfSearchResults = searchSettings?.ShowPdfSearchResults ?? true;

            return View("_QuickSearchAll", model);
        }

        [ChildActionOnly]
        public ActionResult Facet(SearchResultPage currentPage, FilterOptionViewModel viewModel) => PartialView("_Facet", viewModel);

        public class AssetPreloadLink
        {
            public enum AssetType
            {
                Unknown = 0,
                Script = 100,
                Style = 200,
                Image = 300,
                Auto = 400
            }

            private const string Format = "<{0}>; rel=preload; as={1}";
            public string Url { get; set; }
            public AssetType Type { get; set; }
            public bool NoPush { get; set; }

            public AssetPreloadLink()
            {
                Type = AssetType.Auto;
            }

            public AssetPreloadLink(AssetType type)
            {
                Type = type;
            }

            public override string ToString()
            {
                if (Type == AssetType.Auto)
                {
                    if (Url.EndsWith(".js"))
                    {
                        Type = AssetType.Script;
                    }
                    else if (Url.EndsWith(".css"))
                    {
                        Type = AssetType.Style;
                    }
                    else if (Url.EndsWith(".png") || Url.EndsWith(".jpg") || Url.EndsWith(".jpeg"))
                    {
                        Type = AssetType.Image;
                    }
                    else
                    {
                        Type = AssetType.Unknown;
                    }
                }

                if (Type != AssetType.Unknown)
                {
                    var output = string.Format(Format, Url, Type.ToString().ToLowerInvariant());
                    if (NoPush)
                    {
                        return output + "; nopush";
                    }
                    return output;
                }
                return string.Empty;
            }
        }
    }
}