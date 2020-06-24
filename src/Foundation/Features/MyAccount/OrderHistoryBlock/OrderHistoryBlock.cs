using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;

using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.MyAccount.OrderHistoryBlock
{
    [ContentType(DisplayName = "Order History Block",
        GUID = "6b910185-7270-43bf-90e5-fc57cc0d1b5c",
        GroupName = GroupNames.Commerce,
        AvailableInEditMode = true)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-18.png")]
    public class OrderHistoryBlock : FoundationBlockData
    {
        [CultureSpecific]
        [Display(Name = "Main body", GroupName = SystemTabNames.Content)]
        public virtual XhtmlString MainBody { get; set; }
    }
}