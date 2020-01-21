using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Querying.Filters;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Markets;
using Foundation.Commerce.ViewModels;
using Foundation.Find.Cms.Facets;
using Foundation.Find.Cms.Models.Blocks.ProductFilters;
using Foundation.Find.Commerce;
using Foundation.Find.Commerce.ViewModels;
using Foundation.Social.Services;
using Mediachase.Commerce;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Search.ProductSearchBlock
{
    [TemplateDescriptor(Default = true)]
    public class ProductSearchBlockController : BlockController<Commerce.Models.Blocks.ProductSearchBlock>
    {
        private readonly LanguageService _languageService;
        private readonly IReviewService _reviewService;
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyService;
        private readonly ICommerceSearchService _searchService;

        public ProductSearchBlockController(LanguageService languageService,
            IReviewService reviewService,
            ICurrentMarket currentMarket,
            ICurrencyService currencyService,
            ICommerceSearchService searchService)
        {
            _languageService = languageService;
            _reviewService = reviewService;
            _currentMarket = currentMarket;
            _currencyService = currencyService;
            _searchService = searchService;
        }

        // GET: RelatedProductsBlock
        public override ActionResult Index(Commerce.Models.Blocks.ProductSearchBlock currentBlock)
        {
            var currentLang = _languageService.GetCurrentLanguage();

            ProductSearchResults result;
            try
            {
                result = GetSearchResults(currentLang.Name, currentBlock);
            }
            catch (ServiceException)
            {
                return View("FindError");
            }

            if (result == null)
            {
                result = new ProductSearchResults
                {
                    ProductViewModels = Enumerable.Empty<ProductTileViewModel>(),
                    FacetGroups = Enumerable.Empty<FacetGroupOption>()
                };
            }

            MergePriorityProducts(currentBlock, result);

            if (!result.ProductViewModels.Any())
            {
                return View("EmptyResult");
            }

            var productSearchResult = new ProductSearchResultViewModel(currentBlock)
            {
                Heading = currentBlock.Heading,
                ItemsPerRow = currentBlock.ItemsPerRow,
                PaddingStyles = currentBlock.PaddingStyles,
                Products = result.ProductViewModels.ToList()
            };

            return PartialView("~/Features/Search/ProductSearchBlock/Index.cshtml", productSearchResult);
        }

        private void MergePriorityProducts(Commerce.Models.Blocks.ProductSearchBlock currentContent, ProductSearchResults result)
        {
            var products = new List<EntryContentBase>();
            if (currentContent != null)
            {
                products = currentContent.PriorityProducts?.FilteredItems?.Select(x => x.GetContent() as EntryContentBase).ToList() ?? new List<EntryContentBase>();
            }

            products = products.Where(x => !result.ProductViewModels.Any(y => y.Code.Equals(x.Code)))
                .Select(x => x)
                .ToList();

            if (!products.Any())
            {
                return;
            }

            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyService.GetCurrentCurrency();
            var ratings = _reviewService.GetRatings(products.Select(x => x.Code)) ?? null;
            var newList = result.ProductViewModels.ToList();
            newList.InsertRange(0, products.Select(document => document.GetProductTileViewModel(market, currency)));
            result.ProductViewModels = newList;
        }

        private ProductSearchResults GetSearchResults(string language, Commerce.Models.Blocks.ProductSearchBlock productSearchBlock)
        {

            var filterOptions = new CommerceFilterOptionViewModel
            {
                Q = productSearchBlock.SearchTerm,
                PageSize = productSearchBlock.ResultsPerPage,
                Sort = string.Empty,
                FacetGroups = new List<FacetGroupOption>(),
                Page = 1
            };

            var filters = GetFilters(productSearchBlock);
            return _searchService.SearchWithFilters(null, filterOptions, filters);
        }

        private IEnumerable<EPiServer.Find.Api.Querying.Filter> GetFilters(Commerce.Models.Blocks.ProductSearchBlock productSearchBlock)
        {
            var filters = new List<EPiServer.Find.Api.Querying.Filter>();
            if (productSearchBlock.Nodes?.FilteredItems != null && productSearchBlock.Nodes.FilteredItems.Any())
            {
                var nodes = productSearchBlock.Nodes.FilteredItems.Select(x => x.GetContent()).OfType<NodeContent>().ToList();
                var outlines = nodes.Select(x => _searchService.GetOutline(x.Code)).ToList();
                var outlineFilters = outlines.Select(s => new PrefixFilter("Outline$$string.lowercase", s.ToLowerInvariant()))
                    .ToList();

                if (outlineFilters.Count == 1)
                {
                    filters.Add(outlineFilters.First());
                }
                else
                {
                    filters.Add(new OrFilter(outlineFilters.ToArray()));
                }

            }

            if (productSearchBlock.MinPrice > 0 || productSearchBlock.MaxPrice > 0)
            {
                var rangeFilter = RangeFilter.Create("DefaultPrice$$number",
                    productSearchBlock.MinPrice.ToString(),
                    productSearchBlock.MaxPrice == 0 ? double.MaxValue.ToString() : productSearchBlock.MaxPrice.ToString());
                rangeFilter.IncludeUpper = true;
                filters.Add(rangeFilter);
            }

            if (productSearchBlock.BrandFilter != null)
            {
                var brands = productSearchBlock.BrandFilter.Split(',');
                var brandFilters = brands.Select(s => new PrefixFilter("Brand$$string.lowercase", s.ToLowerInvariant())).ToList();
                if (brandFilters.Count == 1)
                {
                    filters.Add(brandFilters.First());
                }
                else
                {
                    filters.Add(new OrFilter(brandFilters.ToArray()));
                }
            }

            if (productSearchBlock.Filters == null)
            {
                return filters;
            }
            foreach (var item in productSearchBlock.Filters.FilteredItems)
            {
                if (item.GetContent() is FilterBaseBlock filter)
                {
                    filters.Add(filter.GetFilter());
                }
            }
            return filters;
        }
    }
}