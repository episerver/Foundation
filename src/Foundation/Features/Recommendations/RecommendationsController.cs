﻿using Foundation.Features.CatalogContent.Services;

namespace Foundation.Features.Recommendations
{
    public class RecommendationsController : Controller
    {
        private readonly IProductService _recommendationService;

        public RecommendationsController(IProductService recommendationService)
        {
            _recommendationService = recommendationService;
        }

        //[ChildActionOnly]
        //public ActionResult Index(IEnumerable<Recommendation> recommendations)
        //{
        //    if (recommendations == null || !recommendations.Any())
        //    {
        //        return new EmptyResult();
        //    }

        //    if (recommendations.Count() > 4)
        //    {
        //        recommendations = recommendations.Take(4);
        //    }

        //    return PartialView("Index", _recommendationService.GetRecommendedProductTileViewModels(recommendations));
        //}
    }
}