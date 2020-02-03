using EPiServer.Core;
using EPiServer.DataAnnotations;
using Foundation.Cms.Media;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Carousel Block",
        GUID = "980ead74-1d13-45d6-9c5c-16f900269ee6",
        Description = "Allows users to create a slider using a collection of Images or Hero blocks",
        GroupName = CmsGroupNames.Content)]
    [ImageUrl("~/assets/icons/cms/blocks/imageslider.png")]
    public class CarouselBlock : FoundationBlockData
    {
        [CultureSpecific]
        [AllowedTypes(new[] { typeof(HeroBlock), typeof(ImageMediaData) })]
        [Display(Name = "Carousel items", Description = "List of carousel items")]
        public virtual ContentArea CarouselItems { get; set; }
    }
}