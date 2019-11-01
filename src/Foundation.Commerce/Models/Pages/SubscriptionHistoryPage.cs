using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Subscription History",
        GUID = "9770edaf-2da0-4522-a446-302d084975c1",
        Description = "Page for customers to view their subscription history",
        GroupName = CommerceGroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-14.png")]
    public class SubscriptionHistoryPage : FoundationPageData
    {
    }
}