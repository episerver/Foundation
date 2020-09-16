using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Reporting.Order.Internal.DataAccess;
using EPiServer.Commerce.Reporting.Order.ReportingModels;
using EPiServer.Core;
using EPiServer.Find;
using EPiServer.Find.Api.Querying.Filters;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Markets;
using Foundation.Commerce.Models.EditorDescriptors;
using Foundation.Features.Blocks.ProductFilterBlocks;
using Foundation.Features.CatalogContent;
using Foundation.Find.Facets;
using Foundation.Social.Services;
using Mediachase.Commerce;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Search.ProductSearchBlock
{
    [TemplateDescriptor(Default = true)]
    public class ProductSearchBlockController : BlockController<ProductSearchBlock>
    {
        private readonly LanguageService _languageService;
        private readonly IReviewService _reviewService;
        private readonly ICurrentMarket _currentMarket;
        private readonly ICurrencyService _currencyService;
        private readonly ISearchService _searchService;
        private readonly ReportingDataLoader _reportingDataLoader;

        public ProductSearchBlockController(LanguageService languageService,
            IReviewService reviewService,
            ICurrentMarket currentMarket,
            ICurrencyService currencyService,
            ISearchService searchService,
            ReportingDataLoader reportingDataLoader)
        {
            _languageService = languageService;
            _reviewService = reviewService;
            _currentMarket = currentMarket;
            _currencyService = currencyService;
            _searchService = searchService;
            _reportingDataLoader = reportingDataLoader;
        }

        public override ActionResult Index(ProductSearchBlock currentBlock)
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

            SortProducts(currentBlock, result);

            MergePriorityProducts(currentBlock, result);

            HandleDiscontinuedProducts(currentBlock, result);

            if (!result.ProductViewModels.Any())
            {
                return View("EmptyResult");
            }

            var productSearchResult = new ProductSearchResultViewModel(currentBlock)
            {
                Heading = currentBlock.Heading,
                ItemsPerRow = currentBlock.ItemsPerRow,
                Products = result.ProductViewModels.ToList()
            };

            return PartialView("~/Features/Search/ProductSearchBlock/Index.cshtml", productSearchResult);
        }

        private void SortProducts(ProductSearchBlock currentContent, ProductSearchResults result)
        {
            var newList = new List<ProductTileViewModel>();

            switch (currentContent.SortOrder)
            {
                case ProductSearchSortOrder.BestSellerByQuantity:
                    var byQuantitys = GetBestSellerByQuantity();
                    newList = result.ProductViewModels.Where(x => !byQuantitys.Any(y => y.Code.Equals(x.Code))).ToList();
                    newList.InsertRange(0, byQuantitys);
                    break;
                case ProductSearchSortOrder.BestSellerByRevenue:
                    var byRevenues = GetBestSellerByRevenue();
                    newList = result.ProductViewModels.Where(x => !byRevenues.Any(y => y.Code.Equals(x.Code))).ToList();
                    newList.InsertRange(0, byRevenues);
                    break;
                case ProductSearchSortOrder.NewestProducts:
                    newList = result.ProductViewModels.OrderByDescending(x => x.Created).ToList();
                    break;
                default:
                    newList = result.ProductViewModels.ToList();
                    break;
            }

            result.ProductViewModels = newList;
        }

        private void MergePriorityProducts(ProductSearchBlock currentContent, ProductSearchResults result)
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

        private void HandleDiscontinuedProducts(ProductSearchBlock currentContent, ProductSearchResults result)
        {
            var newList = new List<ProductTileViewModel>();
            switch (currentContent.DiscontinuedProductsMode)
            {
                case DiscontinuedProductMode.Hide:
                    newList = result.ProductViewModels.Where(x => !x.ProductStatus.Equals("Discontinued")).ToList();
                    break;
                case DiscontinuedProductMode.DemoteToBottom:
                    var discontinueds = result.ProductViewModels.Where(x => x.ProductStatus.Equals("Discontinued")).ToList();
                    var products = result.ProductViewModels.Where(x => !x.ProductStatus.Equals("Discontinued")).ToList();
                    discontinueds.InsertRange(0, products);
                    newList = discontinueds;
                    break;
                default:
                    newList = result.ProductViewModels.ToList();
                    break;
            }

            result.ProductViewModels = newList;
        }

        private IEnumerable<ProductTileViewModel> GetBestSellerByQuantity()
        {
            if (!double.TryParse(ConfigurationManager.AppSettings["episerver:commerce.ReportingTimeRanges"], out var days))
            {
                days = 365;
            }
            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyService.GetCurrentCurrency();
            var lineItems = _reportingDataLoader.GetReportingData(DateTime.Now.AddDays(-days), DateTime.Now);
            var topSeller = new Dictionary<LineItemReportingModel, decimal>();
            foreach (var lineItem in lineItems)
            {
                if (topSeller.ContainsKey(lineItem))
                {
                    topSeller[lineItem] += lineItem.Quantity;
                }
                else
                {
                    topSeller.Add(lineItem, lineItem.Quantity);
                }
            }
            return topSeller.OrderByDescending(x => x.Value).Select(x => x.Key.GetEntryContentBase().GetProductTileViewModel(market, currency));
        }

        private IEnumerable<ProductTileViewModel> GetBestSellerByRevenue()
        {
            if (!double.TryParse(ConfigurationManager.AppSettings["episerver:commerce.ReportingTimeRanges"], out var days))
            {
                days = 365;
            }
            var market = _currentMarket.GetCurrentMarket();
            var currency = _currencyService.GetCurrentCurrency();
            var lineItems = _reportingDataLoader.GetReportingData(DateTime.Now.AddDays(-days), DateTime.Now);
            var topSeller = new Dictionary<LineItemReportingModel, decimal>();
            foreach (var lineItem in lineItems)
            {
                if (topSeller.ContainsKey(lineItem))
                {
                    topSeller[lineItem] += lineItem.ExtendedPrice * lineItem.Quantity;
                }
                else
                {
                    topSeller.Add(lineItem, lineItem.ExtendedPrice * lineItem.Quantity);
                }
            }
            return topSeller.OrderByDescending(x => x.Value).Select(x => x.Key.GetEntryContentBase().GetProductTileViewModel(market, currency));
        }

        private ProductSearchResults GetSearchResults(string language, ProductSearchBlock productSearchBlock)
        {
            var filterOptions = new FilterOptionViewModel
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

        private IEnumerable<EPiServer.Find.Api.Querying.Filter> GetFilters(ProductSearchBlock productSearchBlock)
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