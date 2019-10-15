using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Address Book Page",
        GUID = "5e373eb0-7930-45ca-8564-e695aacd83b4",
        Description = "Manages address book for customer.",
        AvailableInEditMode = false,
        GroupName = CommerceGroupNames.Commerce)]
    [ImageUrl("~/assets/icons/cms/pages/elected.png")]
    public class AddressBookPage : FoundationPageData
    {
    }
}