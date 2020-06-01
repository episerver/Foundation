using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Cms;
using Foundation.Cms.Pages;
using Foundation.Cms.SiteSettings.Pages;
using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.SiteSettings.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.SiteSettings.Pages
{
    [ContentType(DisplayName = "Site Structure Settings Page", 
        GUID = "bf69f959-c91b-46cb-9829-2ecf9d11e13b", 
        Description = "Site structure settings",
        GroupName = CmsGroupNames.Settings)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-structure-settings.png")]
    public class SiteStructureSettingsPage : SettingsBasePage, ISiteStructureSettings
    {
        #region Site Structure

        [Display(Name = "Show product ratings on all product tiles", GroupName = CommerceTabNames.SiteStructure, Order = 5)]
        public virtual bool ShowProductRatingsOnListings { get; set; }

        [AllowedTypes(typeof(SearchResultPage))]
        [Display(Name = "Search page", GroupName = CommerceTabNames.SiteStructure, Order = 10)]
        public virtual ContentReference SearchPage { get; set; }

        [Display(Name = "Store locator page", GroupName = CommerceTabNames.SiteStructure, Order = 20)]
        public virtual ContentReference StoreLocatorPage { get; set; }

        [AllowedTypes(typeof(AddressBookPage))]
        [Display(Name = "Address book page", GroupName = CommerceTabNames.SiteStructure, Order = 30)]
        public virtual ContentReference AddressBookPage { get; set; }

        [AllowedTypes(typeof(ResetPasswordPage))]
        [Display(Name = "Reset password page", GroupName = CommerceTabNames.SiteStructure, Order = 40)]
        public virtual ContentReference ResetPasswordPage { get; set; }

        [AllowedTypes(typeof(WishListPage))]
        [Display(Name = "Wishlist page", GroupName = CommerceTabNames.SiteStructure, Order = 50)]
        public virtual ContentReference WishlistPage { get; set; }

        [AllowedTypes(typeof(CartPage))]
        [Display(Name = "Shopping cart page", GroupName = CommerceTabNames.SiteStructure, Order = 60)]
        public virtual ContentReference CartPage { get; set; }

        [AllowedTypes(typeof(SharedCartPage))]
        [Display(Name = "Shared cart page", GroupName = CommerceTabNames.SiteStructure, Order = 70)]
        public virtual ContentReference SharedCartPage { get; set; }

        [AllowedTypes(typeof(SubscriptionDetailPage))]
        [Display(Name = "Payment plan details page", GroupName = CommerceTabNames.SiteStructure, Order = 80)]
        public virtual ContentReference PaymentPlanDetailsPage { get; set; }

        [AllowedTypes(typeof(SubscriptionHistoryPage))]
        [Display(Name = "Payment plan history page", GroupName = CommerceTabNames.SiteStructure, Order = 90)]
        public virtual ContentReference PaymentPlanHistoryPage { get; set; }

        [AllowedTypes(typeof(OrganizationPage))]
        [Display(Name = "Organization main page", GroupName = CommerceTabNames.SiteStructure, Order = 100)]
        public virtual ContentReference OrganizationMainPage { get; set; }

        [AllowedTypes(typeof(SubOrganizationPage))]
        [Display(Name = "Sub-organization page", GroupName = CommerceTabNames.SiteStructure, Order = 110)]
        public virtual ContentReference SubOrganizationPage { get; set; }

        [Display(Name = "Organization order pads page", GroupName = CommerceTabNames.SiteStructure, Order = 120)]
        [AllowedTypes(typeof(OrderPadsPage))]
        public virtual ContentReference OrganizationOrderPadsPage { get; set; }

        [AllowedTypes(typeof(QuickOrderPage))]
        [Display(Name = "Quick order page", GroupName = CommerceTabNames.SiteStructure, Order = 130)]
        public virtual ContentReference QuickOrderPage { get; set; }

        [AllowedTypes(typeof(OrderDetailsPage))]
        [Display(Name = "Order details page", GroupName = CommerceTabNames.SiteStructure, Order = 140)]
        public virtual ContentReference OrderDetailsPage { get; set; }

        [AllowedTypes(typeof(OrderHistoryPage))]
        [Display(Name = "Order history page", GroupName = CommerceTabNames.SiteStructure, Order = 150)]
        public virtual ContentReference OrderHistoryPage { get; set; }

        [AllowedTypes(typeof(OrderConfirmationPage))]
        [Display(Name = "Order confirmation page", GroupName = CommerceTabNames.SiteStructure, Order = 160)]
        public virtual ContentReference OrderConfirmationPage { get; set; }

        [AllowedTypes(typeof(CheckoutPage))]
        [Display(Name = "Checkout page", GroupName = CommerceTabNames.SiteStructure, Order = 170)]
        public virtual ContentReference CheckoutPage { get; set; }

        [Display(Name = "Resource not found page", GroupName = CommerceTabNames.SiteStructure, Order = 180)]
        public virtual ContentReference PageNotFound { get; set; }

        #endregion

        #region Mail templates

        [Display(Name = "Send order confirmations", GroupName = CommerceTabNames.MailTemplates, Order = 10)]
        public virtual bool SendOrderConfirmationMail { get; set; }

        [AllowedTypes(typeof(MailBasePage))]
        [Display(Name = "Order confirmation", GroupName = CommerceTabNames.MailTemplates, Order = 20)]
        public virtual ContentReference OrderConfirmationMail { get; set; }

        [AllowedTypes(typeof(MailBasePage))]
        [Display(Name = "Reset password", GroupName = CommerceTabNames.MailTemplates, Order = 30)]
        public virtual ContentReference ResetPasswordMail { get; set; }

        #endregion
    }
}