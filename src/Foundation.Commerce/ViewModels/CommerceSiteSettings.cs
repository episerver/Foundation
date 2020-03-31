using EPiServer.Core;
using EPiServer.SpecializedProperties;
using Foundation.Cms.ViewModels;

namespace Foundation.Commerce.ViewModels
{
    public class CommerceSiteSettings : CmsSiteSettings
    {
        public virtual XhtmlString BannerText { get; set; }
        public virtual string MyAccountLabel { get; set; }
        public virtual string CartLabel { get; set; }
        public virtual string SearchLabel { get; set; }
        public virtual string WishlistLabel { get; set; }
        public virtual string SharedCartLabel { get; set; }
        public virtual LinkItemCollection MyAccountCommerceMenu { get; set; }
        public virtual LinkItemCollection OrganizationMenu { get; set; }
        public virtual bool ShowProductRatingsOnListings { get; set; }
        public virtual ContentReference SearchPage { get; set; }
        public virtual ContentReference StoreLocatorPage { get; set; }
        public virtual ContentReference AddressBookPage { get; set; }
        public virtual ContentReference ResetPasswordPage { get; set; }
        public virtual ContentReference WishlistPage { get; set; }
        public virtual ContentReference CartPage { get; set; }
        public virtual ContentReference SharedCartPage { get; set; }
        public virtual ContentReference PaymentPlanDetailsPage { get; set; }
        public virtual ContentReference PaymentPlanHistoryPage { get; set; }
        public virtual ContentReference OrganizationMainPage { get; set; }
        public virtual ContentReference SubOrganizationPage { get; set; }
        public virtual ContentReference OrganizationOrderPadsPage { get; set; }
        public virtual ContentReference QuickOrderPage { get; set; }
        public virtual ContentReference OrderDetailsPage { get; set; }
        public virtual ContentReference OrderHistoryPage { get; set; }
        public virtual ContentReference OrderConfirmationPage { get; set; }
        public virtual ContentReference CheckoutPage { get; set; }
        public virtual ContentReference PageNotFound { get; set; }
        public virtual bool SendOrderConfirmationMail { get; set; }
        public virtual ContentReference OrderConfirmationMail { get; set; }
        public virtual ContentReference ResetPasswordMail { get; set; }
    }
}
