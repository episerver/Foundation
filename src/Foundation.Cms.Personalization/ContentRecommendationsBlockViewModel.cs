using Foundation.Cms.Pages;
using System.Collections.Generic;

namespace Foundation.Cms.Personalization
{
    public class ContentRecommendationsBlockViewModel
    {
        public List<ContentRecommendationItem> ContentRecommendationItems { get; set; }
    }

    public class ContentRecommendationItem
    {
        public FoundationPageData Content { get; set; }
        public string ContentUrl { get; set; }
        public string ContentType { get; set; }
    }
}
