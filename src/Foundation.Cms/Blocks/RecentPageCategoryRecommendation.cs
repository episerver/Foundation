using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Recent Page Category Recommendation", GUID = "d1728a48-764a-4a02-bfb6-4e004fb4ac92", GroupName = "Personalization")]
    [SiteImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-23.png")]
    public class RecentPageCategoryRecommendation : FoundationBlockData
    {
        [Display(Name = "Number of recommendations")]
        public virtual int NumberOfRecommendations { get; set; }

        [Display(Name = "Inspiration folder")]
        public virtual ContentReference InspirationFolder { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            NumberOfRecommendations = 2;
        }
    }
}