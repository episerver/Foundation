using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Container Block", GUID = "8bdfac81-1dbd-43b9-a012-522bd67ee8b3", GroupName = CmsGroupNames.Content)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-04.png")]
    public class ContainerBlock : FoundationBlockData
    {
        [Display(Name = "Main content area")]
        public virtual ContentArea MainContentArea { get; set; }

        [Display(Name = "Wrapping class")]
        public virtual string WrappingClass { get; set; }
    }
}