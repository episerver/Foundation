using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Features.Shared.EditorDescriptors;
using Foundation.Infrastructure;

namespace Foundation.Features.NamedCarts.SharedCart
{
    [ContentType(DisplayName = "Shared Cart Page",
        GUID = "701b5df0-fa41-40cb-807f-645be22714cc",
        Description = "Page to manage organization's shared cart.",
        GroupName = GroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-08.png")]
    public class SharedCartPage : FoundationPageData, IDisableOPE
    {
    }
}