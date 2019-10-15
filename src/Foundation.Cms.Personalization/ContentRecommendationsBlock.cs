using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.Blocks;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Personalization
{
    [ContentType(DisplayName = "Content Recommendations Block",
        GUID = "05e741ce-3d7d-4d62-ba2c-c4d94556534a",
        Description = "List of recommendation contents of the parent content",
        GroupName = CmsPersonalizationGroupNames.CmsPersonalization)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-24.png")]
    public class ContentRecommendationsBlock : FoundationBlockData
    {
        [Display(Name = "Number of recommendations")]
        public virtual int NumberOfRecommendations { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            NumberOfRecommendations = 5;
        }
    }
}