using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms.EditorDescriptors;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Recent Page Category Recommendation Block",
        GUID = "d1728a48-764a-4a02-bfb6-4e004fb4ac92",
        Description = "Block that show recommendations based on selected recent page category",
        GroupName = "Personalization")]
    [SiteImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-23.png")]
    public class RecentPageCategoryRecommendationBlock : FoundationBlockData
    {
        [Display(Name = "Number of recommendations")]
        public virtual int NumberOfRecommendations { get; set; }

        [Display(Name = "Filter root")]
        public virtual ContentReference FilterRoot { get; set; }

        [Display(Name = "Filter types")]
        [SelectMany(SelectionFactoryType = typeof(AvailablePageTypesSelectionFactory))]
        public virtual string FilterTypes { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            NumberOfRecommendations = 2;
        }
    }
}