using EPiServer.Find.Statistics.Api;
using Foundation.Features.CatalogContent;
using Foundation.Find.Facets;
using System.Collections.Generic;

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
