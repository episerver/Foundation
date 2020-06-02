using EPiServer.Core;
using Foundation.Commerce.SiteSettings.Interfaces;

namespace Foundation.Commerce.SiteSettings.Models
{
    public class SiteStructureSettingsModel : ISiteStructureSettings
    {
        public bool ShowProductRatingsOnListings { get; set; }
        public ContentReference SearchPage { get; set; }
        public ContentReference StoreLocatorPage { get; set; }
        public ContentReference AddressBookPage { get; set; }
        public ContentReference ResetPasswordPage { get; set; }
        public ContentReference WishlistPage { get; set; }
        public ContentReference CartPage { get; set; }
        public ContentReference SharedCartPage { get; set; }
        public ContentReference PaymentPlanDetailsPage { get; set; }
        public ContentReference PaymentPlanHistoryPage { get; set; }
        public ContentReference OrganizationMainPage { get; set; }
        public ContentReference SubOrganizationPage { get; set; }
        public ContentReference OrganizationOrderPadsPage { get; set; }
        public ContentReference QuickOrderPage { get; set; }
        public ContentReference OrderDetailsPage { get; set; }
        public ContentReference OrderHistoryPage { get; set; }
        public ContentReference OrderConfirmationPage { get; set; }
        public ContentReference CheckoutPage { get; set; }
        public ContentReference PageNotFound { get; set; }
        public bool SendOrderConfirmationMail { get; set; }
        public ContentReference OrderConfirmationMail { get; set; }
        public ContentReference ResetPasswordMail { get; set; }
    }
}
