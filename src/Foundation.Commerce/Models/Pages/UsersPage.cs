using EPiServer.DataAnnotations;
using Foundation.Cms.EditorDescriptors;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Users Page",
        GUID = "8118b44f-17d9-47af-a40c-c77d1aa0d2ae",
        Description = "Page to manage an organization's users.",
        AvailableInEditMode = false,
        GroupName = CommerceGroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/elected.png")]
    public class UsersPage : FoundationPageData, IDisableOPE
    {
    }
}