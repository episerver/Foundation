using Foundation.Features.Shared.EditorDescriptors;

namespace Foundation.Features.MyOrganization.Organization
{
    [ContentType(DisplayName = "Organization Page",
        GUID = "e50f0e69-0851-40dc-b00c-38f0acec3f32",
        Description = "Page to manage an organization",
        AvailableInEditMode = false,
        GroupName = GroupNames.Commerce)]
    [ImageUrl("/icons/cms/pages/elected.png")]
    public class OrganizationPage : FoundationPageData, IDisableOPE
    {
    }
}