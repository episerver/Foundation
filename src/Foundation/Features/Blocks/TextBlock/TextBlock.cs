using EPiServer.Core;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.TextBlock
{
    [ContentType(DisplayName = "Text Block",
        GUID = "32782B29-278B-410A-A402-9FF46FAF32B9",
        Description = "Simple Rich Text Block",
        GroupName = GroupNames.Content)]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-03.png")]
    public class TextBlock : FoundationBlockData
    {
        [CultureSpecific]
        [Display(Name = "Main body")]
        public virtual XhtmlString MainBody { get; set; }
    }
}