using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;

namespace Foundation.Features.MyAccount.OrderHistory
{
    [ContentType(DisplayName = "Order History Page",
        GUID = "6b950185-7270-43bf-90e5-fc57cc0d1b5c",
        Description = "Page for customer to view their order history.",
        GroupName = GroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-15.png")]
    public class OrderHistoryPage : FoundationPageData
    {
    }
}