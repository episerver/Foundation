using EPiServer.DataAnnotations;
using Foundation.Cms.EditorDescriptors;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Shared Cart Page",
        GUID = "701b5df0-fa41-40cb-807f-645be22714cc",
        Description = "Page to manage organization's shared cart.",
        GroupName = CommerceGroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-08.png")]
    public class SharedCartPage : FoundationPageData, IDisableOPE
    {
    }
}