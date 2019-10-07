using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using EPiServer.Web.Mvc.Html;
using Foundation.Cms.Pages;
using Foundation.Cms.Personalization;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Commerce.Personalization;
using Foundation.Demo.Models;
using Foundation.Demo.ViewModels;
using Foundation.Find.Cms;
using Foundation.Find.Cms.ViewModels;
using Foundation.Find.Commerce;
using Foundation.Find.Commerce.ViewModels;
using Mediachase.Commerce.Catalog;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace {projectname}.Features.Search
{
    public class SearchController : PageController<SearchPage>
    {
        private readonly ISearchViewModelFactory _viewModelFactory;
        private readonly ICommerceSearchService _commerceSearchService;
        private readonly ICmsSearchService _cmsSearchService;
        private readonly ICommerceTrackingService _recommendationService;
        private readonly ReferenceConverter _referenceConverter;
        private readonly ICmsTrackingService _cmsTrackingService;
        private readonly HttpContextBase _httpContextBase;
        private readonly IContentLoader _contentLoader;

        public SearchController(
            ISearchViewModelFactory viewModelFactory,
            ICommerceSearchService searchService,
            ICommerceTrackingService recommendationService,
            ReferenceConverter referenceConverter,
            HttpContextBase httpContextBase,
            IContentLoader contentLoader,
            ICmsTrackingService cmsTrackingService,
            ICmsSearchService cmsSearchService)
        {
            _viewModelFactory = viewModelFactory;
            _commerceSearchService = searchService;
            _recommendationService = recommendationService;
            _referenceConverter = referenceConverter;
            _cmsTrackingService = cmsTrackingService;
            _httpContextBase = httpContextBase;
            _contentLoader = contentLoader;
            _cmsSearchService = cmsSearchService;
        }

        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public async Task<ActionResult> Index(SearchPage currentPage, CommerceFilterOptionViewModel filterOptions)
        {
            if (filterOptions == null)
            {
                return Redirect(Url.ContentUrl(ContentReference.StartPage));
            }

            var startPage = _contentLoader.Get<DemoHomePage>(ContentReference.StartPage);
            var viewModel = _viewModelFactory.Create<DemoSearchViewModel<SearchPage>, SearchPage>(currentPage, new CommerceArgs
            {
                FilterOption = filterOptions,
                SelectedFacets = HttpContext.Request.QueryString["facets"],
                CatalogId = startPage.SearchCatalog
            });

            if (viewModel == null)
            {
                return View(viewModel);
            }

            if (!startPage.ShowProductSearchResults)
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
            if (startPage.ShowContentSearchResults)
            {
                viewModel.UnifiedSearchResults = _cmsSearchService.SearchContent(new CmsFilterOptionViewModel()
                {
                    Q = filterOptions.Q,
                    PageSize = 5,
                    Page = filterOptions.SearchContent ? filterOptions.Page : 1,
                    SectionFilter = filterOptions.SectionFilter
                });
            }

            var productCount = viewModel.ProductViewModels?.Count() ?? 0;
            var contentCount = viewModel.UnifiedSearchResults?.Count() ?? 0;

            if (productCount + contentCount == 1)
            {
                if (productCount == 1)
                {
                    var product = viewModel.ProductViewModels.FirstOrDefault();
                    return Redirect(product.Url);
                }
                else
                {
                    var content = viewModel.UnifiedSearchResults.FirstOrDefault();
                    return Redirect(content.Url);
                }
            }

            return View(viewModel);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult QuickSearch(string search = "")
        {
            var redirectUrl = "";
            var startPage = _contentLoader.Get<DemoHomePage>(ContentReference.StartPage);
            var productCount = 0;
            var contentCount = 0;

            var model = new DemoSearchViewModel<SearchPage>();

            if (startPage.ShowProductSearchResults)
            {
                var productResults = _commerceSearchService.QuickSearch(search);
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

            if (startPage.ShowContentSearchResults)
            {
                var contentResult = _cmsSearchService.SearchContent(new CmsFilterOptionViewModel()
                {
                    Q = search,
                    PageSize = 5,
                    Page = 1,
                });
                model.UnifiedSearchResults = contentResult;
                contentCount = contentResult?.Count() ?? 0;
            }

            if (productCount + contentCount == 1)
            {
                if (productCount == 1)
                {
                    model.RedirectUrl = redirectUrl;
                }
                else
                {
                    model.RedirectUrl = redirectUrl;
                }
            }

            model.ShowContentSearchResults = startPage.ShowContentSearchResults;
            model.ShowProductSearchResults = startPage.ShowProductSearchResults;

            return View("_QuickSearchAll", model);
        }

        [ChildActionOnly]
        public ActionResult Facet(SearchPage currentPage, CommerceFilterOptionViewModel viewModel) => PartialView("_Facet", viewModel);


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
