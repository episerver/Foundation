using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Gift Card Page",
        GUID = "845a7ade-4cac-4efd-86fd-a71ac3cfa2b6",
        Description = "This page displays all gift cards belonging to an user",
        GroupName = CommerceGroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-12.png")]
    public class GiftCardPage : FoundationPageData
    {
    }
}