using Foundation.Features.Home;
using Foundation.Features.MyAccount.OrderConfirmation;

namespace Foundation.Features.Checkout
{
    [ContentType(DisplayName = "Checkout Page",
        GUID = "6709cd32-7bb6-4d29-9b0b-207369799f4f",
        Description = "Checkout page",
        GroupName = GroupNames.Commerce,
        AvailableInEditMode = false)]
    [AvailableContentTypes(Include = new[] { typeof(OrderConfirmationPage) }, IncludeOn = new[] { typeof(HomePage) })]
    [ImageUrl("/icons/cms/pages/CMS-icon-page-08.png")]
    public class CheckoutPage : FoundationPageData
    {
    }
}