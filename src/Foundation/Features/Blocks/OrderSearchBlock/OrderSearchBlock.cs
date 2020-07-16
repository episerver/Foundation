using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;

namespace Foundation.Features.Blocks.OrderSearchBlock
{
    [ContentType(DisplayName = "Order Search Block",
        GUID = "dd74d77f-3dce-4956-87fc-39bdbeebaf9c",
        Description = "A block that allows to search/filter on orders",
        GroupName = GroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-33.png")]
    public class OrderSearchBlock : FoundationBlockData
    {
    }
}