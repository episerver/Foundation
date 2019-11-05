using EPiServer.DataAnnotations;
using Foundation.Cms.EditorDescriptors;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Order Pads Page",
        GUID = "32114883-3ebb-4582-b864-7262ea177af0",
        Description = "Page to manage an organization member's order pad",
        GroupName = CommerceGroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-15.png")]
    public class OrderPadsPage : FoundationPageData, IDisableOPE
    {
    }
}