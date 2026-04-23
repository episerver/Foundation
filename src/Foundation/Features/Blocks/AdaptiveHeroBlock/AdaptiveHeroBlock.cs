using AdaptiveImages.Core;
using AdaptiveImages.Models;
using AdaptiveImages.Validation;
using Foundation.Features.Blocks.HeroBlock;

namespace Foundation.Features.Blocks.AdaptiveHeroBlock
{
    [ContentType(DisplayName = "Adaptive Hero Block",
        GUID = "{901DD891-97F1-4230-BE94-61285A2D1B16}",
        Description = "Adaptive image block with overlay for text",
        GroupName = GroupNames.Content)]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-22.png")]
    public class AdaptiveHeroBlock : FoundationBlockData//, IDashboardItem
    {
        [SelectOne(SelectionFactoryType = typeof(BlockRatioSelectionFactory))]
        [Display(Name = "Block ratio (width:height)", Order = 5)]
        public virtual string BlockRatio { get; set; }

        [CultureSpecificImage]
        [Display(Name = "Image", GroupName = SystemTabNames.Content, Order = 10)]
        [Proportions(AllowCropping = true, AllowFocalPoint = true)]
        public virtual SingleImage HeroImage { get; set; }

        [CultureSpecificImage]
        [Display(Name = "Image (Adaptive) (Primary)",
         Order = 15,
         GroupName = SystemTabNames.Content,
         Description = "If set, Adaptive Image takes priority over single image.")]
        [Size(1280, FormFactor.Large)]
        [Proportions(3, 2, FormFactor.Large, "Landscape")]
        [Proportions(16, 9, FormFactor.Large, "Widescreen", IsDefault = true, AllowCropping = true, AllowFocalPoint = true)]
        // Portrait proportions at least 1280x960 for tablet
        [Size(768, FormFactor.Medium)]
        [Proportions(4, 3, FormFactor.Medium, AllowCropping = true, AllowFocalPoint = true)]
        // Square image at least 768x768 for mobile
        [Size(540, FormFactor.Small)]
        [Proportions(1, 1, FormFactor.Small, AllowCropping = true, AllowFocalPoint = true)]
        public virtual AdaptiveImage HeroAdaptiveImage { get; set; }

        [CultureSpecific]
        [UIHint(UIHint.Video)]
        [Display(Name = "Video", Order = 20)]
        public virtual ContentReference MainBackgroundVideo { get; set; }

        [Display(Order = 30)]
        public virtual Url Link { get; set; }

        [UIHint("HeroBlockCallout")]
        [Display(Name = "Callout", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual HeroBlockCallout Callout { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            BlockOpacity = 1;
            BlockRatio = "2:1";
        }
    }
}