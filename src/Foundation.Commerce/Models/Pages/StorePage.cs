using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Store Page",
        GUID = "77cf19e8-9a94-4c5b-a9be-ece53de563dc",
        Description = "Store locator page.",
        GroupName = CommerceGroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-22.png")]
    public class StorePage : FoundationPageData
    {
    }
}