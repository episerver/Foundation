using EPiServer.Core;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Full Width Container Block",
    GUID = "9BA18CF6-A1EA-409C-A8C7-F19D88656068",
    Description = "The container block that is full width inside container class",
    GroupName = CmsGroupNames.Content)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-34.png")]
    public class FullWidthContainerBlock : FoundationBlockData
    {
        [Display(Name = "Content area", Order = 20)]
        public virtual ContentArea ContentArea { get; set; }
    }
}
