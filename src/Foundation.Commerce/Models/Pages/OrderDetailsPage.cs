using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Order Details Page",
        GUID = "11ad9718-fc02-45d0-9b98-349da9493dce",
        Description = "Page for customer to view their order",
        GroupName = CommerceGroupNames.Commerce,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-15.png")]
    public class OrderDetailsPage : FoundationPageData
    {
    }
}