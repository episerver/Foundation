using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Sales Rep Page",
        GUID = "37456ce5-5277-4154-9e79-6d2ec1de0d32",
        Description = "Page for sales rep to be able to manage their accounts",
        GroupName = CommerceGroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/elected.png")]
    public class SalesRepPage : FoundationPageData
    {
    }
}