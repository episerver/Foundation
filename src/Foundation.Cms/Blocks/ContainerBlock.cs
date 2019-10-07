using EPiServer.Core;
using EPiServer.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Container Block", GUID = "8bdfac81-1dbd-43b9-a012-522bd67ee8b3", Description = "", GroupName = CmsTabs.Content)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-04.png")]
    public class ContainerBlock : FoundationBlockData
    {
        public virtual ContentArea MainContentArea { get; set; }

        public virtual string WrappingClass { get; set; }
    }
}