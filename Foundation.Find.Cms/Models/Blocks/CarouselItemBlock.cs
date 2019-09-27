using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Foundation.Cms.Blocks;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Find.Cms.Models.Blocks
{
    [ContentType(
        DisplayName = "Carousel Item Block",
        GUID = "d6c6b451-0e31-4cb8-aa0f-15c529cb4f34",
        Description = "",
        GroupName = FindTabs.Location)]
    [ImageUrl("~/assets/icons/cms/blocks/map.png")]
    public class CarouselItemBlock : FoundationBlockData
    {
        [CultureSpecific]
        [Display(
            Name = "Title",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 100)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Subtitle",
            Description = "Second heading line",
            GroupName = SystemTabNames.Content,
            Order = 110)]
        public virtual string SubTitle { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Description",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 130)]
        public virtual string MainBody { get; set; }

        [Display(
            Name = "Image",
            Description = "",
            GroupName = SystemTabNames.Content,
            Order = 140)]
        [UIHint(UIHint.Image)]
        public virtual ContentReference Image { get; set; }
    }
}
