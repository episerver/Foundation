using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Foundation.Features.Shared;
using Foundation.Infrastructure;

namespace Foundation.Features.Blocks.IframeBlock
{
    [ContentType(DisplayName = "Iframe block",
        GUID = "6b66cf33-75b3-4f34-a775-c319f3ab5130",
        GroupName = GroupNames.Content)]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-22.png")]
    public class IframeBlock : FoundationBlockData
    {
        [Required]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Url { get; set; }

        [SelectOne(SelectionFactoryType = typeof(RatioSelectionFactory))]
        [Display(Name = "Ratio (width:height)", Order = 20)]
        public virtual string IframeRatio { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            IframeRatio = "ratio-16x9";
        }
    }
}