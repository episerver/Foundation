using EPiServer.Commerce.Catalog.ContentTypes;
using Foundation.Features.Blocks.ProductFilterBlocks;
using Foundation.Features.CatalogContent;
using Foundation.Infrastructure.Find.Facets;

namespace Foundation.Features.Search.ProductSearchBlock
{
    public class ProductSearchBlockComponent : AsyncBlockComponent<ProductSearchBlock>
    {
        private readonly ISearchService _searchService;
        private readonly IContentLoader _contentLoader;

        public ProductSearchBlockComponent(ISearchService searchService, IContentLoader contentLoader)
        {
            _searchService = searchService;
            _contentLoader = contentLoader;
        }

        protected override async Task<IViewComponentResult> InvokeComponentAsync(ProductSearchBlock currentBlock)
        {
            var filterOptions = new FilterOptionViewModel
            {
                Q = currentBlock.SearchTerm,
                PageSize = currentBlock.ResultsPerPage > 0 ? currentBlock.ResultsPerPage : 6,
                Sort = string.Empty,
                FacetGroups = new List<FacetGroupOption>(),
                Page = 1
            };

            // Use the first configured catalog node as the search root so results
            // are scoped to the block's assigned category (e.g. Womens, Mens).
            // Falls back to null (catalog root) when no node is configured.
            IContent searchRoot = null;
            if (currentBlock.Nodes?.Items != null)
            {
                foreach (var item in currentBlock.Nodes.Items)
                {
                    try
                    {
                        var node = _contentLoader.Get<NodeContent>(item.ContentLink);
                        if (node != null) { searchRoot = node; break; }
                    }
                    catch { }
                }
            }

            // Collect filter predicates from any FilterBaseBlock instances in the Filters ContentArea.
            var predicates = new List<Func<EntryContentBase, bool>>();
            if (currentBlock.Filters?.Items != null)
            {
                foreach (var item in currentBlock.Filters.Items)
                {
                    try
                    {
                        var filterBlock = _contentLoader.Get<FilterBaseBlock>(item.ContentLink);
                        var predicate = filterBlock?.GetPredicate();
                        if (predicate != null) predicates.Add(predicate);
                    }
                    catch { }
                }
            }

            var result = _searchService.Search(searchRoot, filterOptions, string.Empty, filters: predicates.Count > 0 ? predicates : null) ?? new ProductSearchResults
            {
                ProductViewModels = Enumerable.Empty<ProductTileViewModel>(),
                FacetGroups = Enumerable.Empty<FacetGroupOption>()
            };

            if (!result.ProductViewModels.Any())
            {
                return await Task.FromResult(View("~/Features/Search/ProductSearchBlock/EmptyResult.cshtml"));
            }

            var productSearchResult = new ProductSearchResultViewModel(currentBlock)
            {
                Heading = currentBlock.Heading,
                ItemsPerRow = currentBlock.ItemsPerRow,
                Products = result.ProductViewModels.ToList()
            };

            return await Task.FromResult(View("~/Features/Search/ProductSearchBlock/Index.cshtml", productSearchResult));
        }
    }
}
