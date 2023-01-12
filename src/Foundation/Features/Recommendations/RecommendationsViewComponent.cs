using EPiServer.Personalization.Commerce.Tracking;
using Foundation.Features.CatalogContent;
using Foundation.Features.CatalogContent.Services;

namespace Foundation.Features.Recommendations
{
    public class RecommendationsViewComponent : ViewComponent
    {
        private readonly IProductService _recommendationService;

        public RecommendationsViewComponent(IProductService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        public IViewComponentResult Invoke(IEnumerable<Recommendation> recommendations)
        {
            if (recommendations == null || !recommendations.Any())
            {
                return View("/Features/Recommendations/Index.cshtml", new List<RecommendedProductTileViewModel>()); ;
            }

            if (recommendations.Count() > 4)
            {
                recommendations = recommendations.Take(4);
            }

            return View("/Features/Recommendations/Index.cshtml", _recommendationService.GetRecommendedProductTileViewModels(recommendations));
        }
    }
}