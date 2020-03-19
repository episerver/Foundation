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
        #region Menu
        [CultureSpecific]
        [Display(Name = "My account menu (commerce)",
            Description = "This menu will show if show commerce components in header is true.",
            GroupName = CmsTabNames.Menu,
            Order = 30)]
        public virtual LinkItemCollection MyAccountCommerceMenu { get; set; }

        [CultureSpecific]
        [Display(Name = "Organization menu", GroupName = CmsTabNames.Menu, Order = 50)]
        public virtual LinkItemCollection OrganizationMenu { get; set; }

        #endregion

        #region Header

        [CultureSpecific]
        [Display(Name = "Banner text", GroupName = CmsTabNames.Header, Order = 20)]
        public virtual XhtmlString BannerText { get; set; }

        #endregion

        #region Site Labels

        [Display(Name = "My account", GroupName = CommerceTabNames.SiteLabels, Order = 10)]
        public virtual string MyAccountLabel { get; set; }

        [Display(Name = "Shopping cart", GroupName = CommerceTabNames.SiteLabels, Order = 20)]
        public virtual string CartLabel { get; set; }

        [Display(Name = "Search", GroupName = CommerceTabNames.SiteLabels, Order = 30)]
        public virtual string SearchLabel { get; set; }

        [Display(Name = "Wishlist", GroupName = CommerceTabNames.SiteLabels, Order = 40)]
        public virtual string WishlistLabel { get; set; }

        [Display(Name = "Shared cart", GroupName = CommerceTabNames.SiteLabels, Order = 50)]
        public virtual string SharedCartLabel { get; set; }

        #endregion

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