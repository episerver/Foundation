using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;

namespace Foundation.Features.MyAccount.SubscriptionDetail
{
    [ContentType(DisplayName = "Subscription Details",
        GUID = "8eaf6fe8-3bf3-4f54-9b4a-06a1569087e1",
        Description = "Page for customer to see their subscription details.",
        GroupName = GroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-14.png")]
    public class SubscriptionDetailPage : FoundationPageData
    {
    }
}