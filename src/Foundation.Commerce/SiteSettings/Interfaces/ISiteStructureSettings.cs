using EPiServer.Core;

namespace Foundation.Commerce.SiteSettings.Interfaces
{
    public interface ISiteStructureSettings
    {
        bool ShowProductRatingsOnListings { get; set; }
        ContentReference SearchPage { get; set; }
        ContentReference StoreLocatorPage { get; set; }
        ContentReference AddressBookPage { get; set; }
        ContentReference ResetPasswordPage { get; set; }
        ContentReference WishlistPage { get; set; }
        ContentReference CartPage { get; set; }
        ContentReference SharedCartPage { get; set; }
        ContentReference PaymentPlanDetailsPage { get; set; }
        ContentReference PaymentPlanHistoryPage { get; set; }
        ContentReference OrganizationMainPage { get; set; }
        ContentReference SubOrganizationPage { get; set; }
        ContentReference OrganizationOrderPadsPage { get; set; }
        ContentReference QuickOrderPage { get; set; }
        ContentReference OrderDetailsPage { get; set; }
        ContentReference OrderHistoryPage { get; set; }
        ContentReference OrderConfirmationPage { get; set; }
        ContentReference CheckoutPage { get; set; }
        ContentReference PageNotFound { get; set; }
        bool SendOrderConfirmationMail { get; set; }
        ContentReference OrderConfirmationMail { get; set; }
        ContentReference ResetPasswordMail { get; set; }
    }
}
