using Foundation.Features.Shared.EditorDescriptors;

namespace Foundation.Features.MyOrganization.Users
{
    [ContentType(DisplayName = "Users Page",
        GUID = "8118b44f-17d9-47af-a40c-c77d1aa0d2ae",
        Description = "Page to manage an organization's users.",
        AvailableInEditMode = false,
        GroupName = GroupNames.Commerce)]
    [ImageUrl("/icons/cms/pages/elected.png")]
    public class UsersPage : FoundationPageData, IDisableOPE
    {
    }
}