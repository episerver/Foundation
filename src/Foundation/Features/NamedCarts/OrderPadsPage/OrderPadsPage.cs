using Foundation.Features.Shared.EditorDescriptors;

namespace Foundation.Features.NamedCarts.OrderPadsPage
{
    [ContentType(DisplayName = "Order Pads Page",
        GUID = "32114883-3ebb-4582-b864-7262ea177af0",
        Description = "Page to manage an organization member's order pad",
        GroupName = GroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-15.png")]
    public class OrderPadsPage : FoundationPageData, IDisableOPE
    {
    }
}