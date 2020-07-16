using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;

namespace Foundation.Features.MyAccount.SubscriptionHistory
{
    [ContentType(DisplayName = "Subscription History",
        GUID = "9770edaf-2da0-4522-a446-302d084975c1",
        Description = "Page for customers to view their subscription history",
        GroupName = GroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-14.png")]
    public class SubscriptionHistoryPage : FoundationPageData
    {
    }
}