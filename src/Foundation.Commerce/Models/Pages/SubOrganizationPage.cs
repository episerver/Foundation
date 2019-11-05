using EPiServer.DataAnnotations;
using Foundation.Cms.EditorDescriptors;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Sub Organization Page",
        GUID = "9699e421-1e17-4590-a66b-d41b1058eaa1",
        Description = "Manage a sub organization",
        AvailableInEditMode = false,
        GroupName = CommerceGroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/elected.png")]
    public class SubOrganizationPage : FoundationPageData, IDisableOPE
    {
    }
}