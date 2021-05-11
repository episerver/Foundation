using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.Web;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.VideoBlock
{
    [ContentType(DisplayName = "Video Block",
        GUID = "03D454F9-3BE8-4421-9A5D-CBBE8E38443D",
        Description = "Video Block",
        GroupName = GroupNames.Content)]
    [ImageUrl("/icons/cms/blocks/CMS-icon-block-05.png")]
    public class VideoBlock : FoundationBlockData
    {
        [CultureSpecific]
        [UIHint(UIHint.Video)]
        public virtual ContentReference Video { get; set; }
    }
}