using EPiServer.Personalization.Commerce.Extensions;
using EPiServer.Personalization.Commerce.Tracking;
using EPiServer.Tracking.Commerce.Data;

namespace Foundation.Infrastructure.Personalization
{
    public static class RecommendationsExtensions
    {
        public const string ProductAlternatives = "productAlternativesWidget";
        public const string ProductCrossSells = "productCrossSellsWidget";
        public const string Home = "homeWidget";
        public const string Category = "categoryWidget";
        public const string SearchResult = "searchWidget";
        public const string Basket = "basketWidget";

        public static IEnumerable<Recommendation> GetAlternativeProductsRecommendations(this Controller controller)
        {
            return controller.GetRecommendationGroups()
                .Where(x => x.Area == ProductAlternatives)
                .SelectMany(x => x.Recommendations);
        }

        public static IEnumerable<Recommendation> GetCrossSellProductsRecommendations(this Controller controller)
        {
            return controller.GetRecommendationGroups()
                .Where(x => x.Area == ProductCrossSells)
                .SelectMany(x => x.Recommendations);
        }

        public static IEnumerable<Recommendation> GetHomeRecommendations(this Controller controller)
        {
            return controller.GetRecommendationGroups()
                .Where(x => x.Area == Home)
                .SelectMany(x => x.Recommendations);
        }

        public static IEnumerable<Recommendation> GetCategoryRecommendations(this TrackingResponseData response, ReferenceConverter referenceConverter) => response.GetRecommendationGroups(referenceConverter).Where(x => x.Area == Category).SelectMany(x => x.Recommendations);

        public static IEnumerable<Recommendation> GetSearchResultRecommendations(this TrackingResponseData response, ReferenceConverter referenceConverter) => response.GetRecommendationGroups(referenceConverter).Where(x => x.Area == SearchResult).SelectMany(x => x.Recommendations);

        public static IEnumerable<Recommendation> GetAlternativeProductsRecommendations(this TrackingResponseData response, ReferenceConverter referenceConverter) => response.GetRecommendationGroups(referenceConverter).Where(x => x.Area == ProductAlternatives).SelectMany(x => x.Recommendations);

        public static IEnumerable<Recommendation> GetCrossSellProductsRecommendations(this TrackingResponseData response, ReferenceConverter referenceConverter) => response.GetRecommendationGroups(referenceConverter).Where(x => x.Area == ProductCrossSells).SelectMany(x => x.Recommendations);

        public static IEnumerable<Recommendation> GetCartRecommendations(this TrackingResponseData response, ReferenceConverter referenceConverter) => response.GetRecommendationGroups(referenceConverter).Where(x => x.Area == Basket).SelectMany(x => x.Recommendations);

        public static IEnumerable<Recommendation> GetRecommendations(this TrackingResponseData response, ReferenceConverter referenceConverter, string area) => response.GetRecommendationGroups(referenceConverter).Where(x => x.Area == area).SelectMany(x => x.Recommendations);
    }
}