using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Features.Shared.Descriptors;
using Foundation.Infrastructure;

namespace Foundation.Features.MyOrganization.SubOrganization
{
    [ContentType(DisplayName = "Sub Organization Page",
        GUID = "9699e421-1e17-4590-a66b-d41b1058eaa1",
        Description = "Manage a sub organization",
        AvailableInEditMode = false,
        GroupName = GroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/elected.png")]
    public class SubOrganizationPage : FoundationPageData, IDisableOPE
    {
    }
}