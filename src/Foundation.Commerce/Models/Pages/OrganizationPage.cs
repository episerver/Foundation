using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.EditorDescriptors;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Organization Page",
        GUID = "e50f0e69-0851-40dc-b00c-38f0acec3f32",
        Description = "Page to manage an organization",
        AvailableInEditMode = false,
        GroupName = CommerceGroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/elected.png")]
    public class OrganizationPage : FoundationPageData, IDisableOPE
    {
    }
}