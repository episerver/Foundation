using EPiServer.DataAnnotations;
using Foundation.Cms.Pages;

namespace Foundation.Commerce.Models.Pages
{
    [ContentType(DisplayName = "Checkout Page",
        GUID = "6709cd32-7bb6-4d29-9b0b-207369799f4f",
        Description = "Checkout page",
        GroupName = CommerceGroupNames.Commerce,
        AvailableInEditMode = false)]
    [AvailableContentTypes(Include = new[] { typeof(OrderConfirmationPage) }, IncludeOn = new[] { typeof(CommerceHomePage) })]
    [ImageUrl("~/assets/icons/cms/pages/cms-icon-page-08.png")]
    public class CheckoutPage : FoundationPageData
    {
    }
}