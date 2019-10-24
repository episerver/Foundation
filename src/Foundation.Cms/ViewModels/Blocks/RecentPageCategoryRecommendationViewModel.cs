using Foundation.Cms.Blocks;
using Foundation.Cms.Pages;
using System.Collections.Generic;

namespace Foundation.Cms.ViewModels.Blocks
{
    public class RecentPageCategoryRecommendationViewModel : BlockViewModel<RecentPageCategoryRecommendationBlock>
    {

        public RecentPageCategoryRecommendationViewModel(RecentPageCategoryRecommendationBlock currentBlock) : base(currentBlock)
        {
        }

        public List<CategoryViewModel> CategoryPages { get; set; }
    }

    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public FoundationPageData Page { get; set; }
    }
}
