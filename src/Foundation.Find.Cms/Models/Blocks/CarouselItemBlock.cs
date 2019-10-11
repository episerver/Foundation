using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Foundation.Cms.Blocks;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Find.Cms.Models.Blocks
{
    [ContentType(DisplayName = "Carousel Item Block", GUID = "d6c6b451-0e31-4cb8-aa0f-15c529cb4f34", GroupName = FindTabNames.Location)]
    [ImageUrl("~/assets/icons/cms/blocks/map.png")]
    public class CarouselItemBlock : FoundationBlockData
    {
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Title { get; set; }

        [CultureSpecific]
        [Display(Description = "Second heading line", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string Subtitle { get; set; }

        [CultureSpecific]
        [Display(Name = "Main Body", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual string MainBody { get; set; }

        [UIHint(UIHint.Image)]
        [Display(GroupName = SystemTabNames.Content, Order = 40)]
        public virtual ContentReference Image { get; set; }
    }
}
