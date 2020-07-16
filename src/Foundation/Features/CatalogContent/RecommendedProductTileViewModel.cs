namespace Foundation.Features.CatalogContent
{
    public class RecommendedProductTileViewModel
    {
        public long RecommendationId { get; }

        public ProductTileViewModel TileViewModel { get; }

        public RecommendedProductTileViewModel(long recommendationId, ProductTileViewModel model)
        {
            RecommendationId = recommendationId;
            TileViewModel = model;
        }
    }
}