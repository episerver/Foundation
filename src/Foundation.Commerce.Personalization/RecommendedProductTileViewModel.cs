using Foundation.Commerce.Catalog.ViewModels;

namespace Foundation.Commerce.Personalization
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