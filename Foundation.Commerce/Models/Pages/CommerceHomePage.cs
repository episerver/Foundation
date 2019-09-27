using EPiServer.Core;
using EPiServer.DataAnnotations;
using EPiServer.SpecializedProperties;
using Foundation.Cms;
using Foundation.Cms.Pages;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Pages
{

    public abstract class CommerceHomePage : CmsHomePage
    {
        [CultureSpecific]
        [Display(
            Name = "My Account Commerce Navigation",
            Description = "This menu will show if show commerce components in header is true.",
            GroupName = CmsTabs.Menu,
            Order = 4)]
        public virtual LinkItemCollection MyAccountCommerceMenu { get; set; }

        [CultureSpecific]
        [Display(
            Name = "Organization Navigation",
            Description = "",
            GroupName = CmsTabs.Menu,
            Order = 6)]
        public virtual LinkItemCollection B2BMenu { get; set; }

        [Display(Name = "Header Banner Text", GroupName = CmsTabs.SiteSettings, Order = 2)]
        public virtual XhtmlString HeaderBannerText { get; set; }

        #region Site Structure

        [Display(
            Name = "Checkout page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 1)]
        [AllowedTypes(typeof(CheckoutPage))]
        public virtual ContentReference CheckoutPage { get; set; }

        [Display(
            Name = "Store locator",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 2)]
        public virtual ContentReference StoreLocator { get; set; }

        [Display(
            Name = "Address book page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 3)]
        [AllowedTypes(typeof(AddressBookPage))]
        public virtual ContentReference AddressBookPage { get; set; }

        [Display(
            Name = "Wish list page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 4)]
        [AllowedTypes(typeof(WishListPage))]
        public virtual ContentReference WishListPage { get; set; }

        [Display(
            Name = "Shared Cart Page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 4)]
        [AllowedTypes(typeof(SharedCartPage))]
        public virtual ContentReference SharedCartPage { get; set; }

        [Display(
            Name = "Search page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 5)]
        [AllowedTypes(typeof(SearchPage))]
        public virtual ContentReference SearchPage { get; set; }

        [Display(
            Name = "Reset password page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 6)]
        [AllowedTypes(typeof(ResetPasswordPage))]
        public virtual ContentReference ResetPasswordPage { get; set; }

        [Display(
            Name = "Payment plan details page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 7)]
        [AllowedTypes(typeof(SubscriptionDetailPage))]
        public virtual ContentReference PaymentPlanDetailsPage { get; set; }

        [Display(
            Name = "Cart page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 8)]
        [AllowedTypes(typeof(CartPage))]
        public virtual ContentReference CartPage { get; set; }

        [Display(
            Name = "Payment plan history page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 9)]
        [AllowedTypes(typeof(SubscriptionHistoryPage))]
        public virtual ContentReference PaymentPlanHistoryPage { get; set; }

        [Display(
            Name = "Organization Main Page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 10)]
        [AllowedTypes(typeof(OrganizationPage))]
        public virtual ContentReference OrganizationMainPage { get; set; }

        [Display(
            Name = "Sub Organization Page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 11)]
        [AllowedTypes(typeof(SubOrganizationPage))]
        public virtual ContentReference SubOrganizationPage { get; set; }

        [Display(
            Name = "Organization Pads Page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 12)]
        [AllowedTypes(typeof(OrderPadsPage))]
        public virtual ContentReference OrderPadsPage { get; set; }

        [Display(
            Name = "Quick Orders Page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 13)]
        [AllowedTypes(typeof(QuickOrderPage))]
        public virtual ContentReference QuickOrderPage { get; set; }

        [Display(
            Name = "Resource not found page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 14)]
        public virtual ContentReference PageNotFound { get; set; }

        [Display(
            Name = "Order details page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 15)]
        [AllowedTypes(typeof(OrderDetailsPage))]
        public virtual ContentReference OrderDetailsPage { get; set; }

        [Display(
            Name = "Order history page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 16)]
        [AllowedTypes(typeof(OrderHistoryPage))]
        public virtual ContentReference OrderHistoryPage { get; set; }

        [Display(
            Name = "Order confirmation page",
            Description = "",
            GroupName = CommerceTabs.SiteStructure,
            Order = 16)]
        [AllowedTypes(typeof(OrderConfirmationPage))]
        public virtual ContentReference OrderConfirmationPage { get; set; }
        #endregion


        #region Labels

        [Display(Name = "My Account Label",
            GroupName = CommerceTabs.Labels,
            Order = 1)]
        public virtual string MyAccountLabel { get; set; }

        [Display(Name = "Cart Label",
            GroupName = CommerceTabs.Labels,
            Order = 2)]
        public virtual string CartLabel { get; set; }

        [Display(Name = "Search Label",
            GroupName = CommerceTabs.Labels,
            Order = 3)]
        public virtual string SearchLabel { get; set; }

        [Display(Name = "Wishlist Label",
            GroupName = CommerceTabs.Labels,
            Order = 4)]
        public virtual string WishlistLabel { get; set; }

        [Display(Name = "Shared Cart Label",
            GroupName = CommerceTabs.Labels,
            Order = 5)]
        public virtual string SharedCartLabel { get; set; }

        #endregion

        #region Mail templates

        [Display(
         Name = "Order confirmation mail",
         Description = "",
         GroupName = CommerceTabs.MailTemplates,
         Order = 1)]
        [AllowedTypes(typeof(MailBasePage))]
        public virtual ContentReference OrderConfirmationMail { get; set; }

        [Display(
         Name = "Send Order Confirmation Mail",
         Description = "",
         GroupName = CommerceTabs.MailTemplates,
         Order = 2)]
        public virtual bool SendOrderConfirmationMail { get; set; }

        [Display(
            Name = "Reset password mail",
            Description = "",
            GroupName = CommerceTabs.MailTemplates,
            Order = 3)]
        public virtual ContentReference ResetPasswordMail { get; set; }

        #endregion

    }
}