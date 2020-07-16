using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;

namespace Foundation.Features.Stores
{
    [ContentType(DisplayName = "Store Page",
        GUID = "77cf19e8-9a94-4c5b-a9be-ece53de563dc",
        Description = "Store locator page.",
        GroupName = GroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-22.png")]
    public class StorePage : FoundationPageData
    {
    }
}