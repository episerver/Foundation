using EPiServer.Personalization.Commerce.Tracking;
using Foundation.Features.CatalogContent.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Recommendations
{
    public class RecommendationsComponent : ViewComponent
    {
        private readonly IProductService _recommendationService;

        public RecommendationsComponent(IProductService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        public IViewComponentResult Invoke(IEnumerable<Recommendation> recommendations)
        {
            if (recommendations == null || !recommendations.Any())
            {
                return View("Index", new List<Recommendation>()); ;
            }

            if (recommendations.Count() > 4)
            {
                recommendations = recommendations.Take(4);
            }

            return View("Index", _recommendationService.GetRecommendedProductTileViewModels(recommendations));
        }
    }
}