using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Wish List Page",
        GUID = "c80ee97b-3151-4602-a447-678534e83a0b",
        Description = "Page for customers to manage their wish list.",
        GroupName = CommerceGroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-08.png")]
    public class WishListPage : FoundationPageData
    {
    }
}