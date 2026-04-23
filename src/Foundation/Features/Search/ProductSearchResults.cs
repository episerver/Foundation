// EPiServer.Find.Statistics.Api removed: DidYouMeanResult replaced with stub from FindTypeStubs.cs.
using Foundation.Features.CatalogContent;
using Foundation.Infrastructure.Find.Facets;

namespace Foundation.Features.Search
{
    public class ProductSearchResults
    {
        public IEnumerable<ProductTileViewModel> ProductViewModels { get; set; }
        public IEnumerable<FacetGroupOption> FacetGroups { get; set; }
        public int TotalCount { get; set; }
        public DidYouMeanResult DidYouMeans { get; set; }
        public string Query { get; set; }
        public string RedirectUrl { get; set; }
    }
}
