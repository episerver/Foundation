using EPiServer.Core;
using EPiServer.DataAnnotations;
using Foundation.Cms.Settings;
using Foundation.Features.Checkout;
using Foundation.Features.Checkout.ConfirmationMail;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyAccount.OrderConfirmation;
using Foundation.Features.MyAccount.OrderDetails;
using Foundation.Features.MyAccount.OrderHistory;
using Foundation.Features.MyAccount.ResetPassword;
using Foundation.Features.MyAccount.SubscriptionDetail;
using Foundation.Features.MyAccount.SubscriptionHistory;
using Foundation.Features.MyOrganization.Organization;
using Foundation.Features.MyOrganization.QuickOrderPage;
using Foundation.Features.MyOrganization.SubOrganization;
using Foundation.Features.NamedCarts.DefaultCart;
using Foundation.Features.NamedCarts.OrderPadsPage;
using Foundation.Features.NamedCarts.SharedCart;
using Foundation.Features.NamedCarts.Wishlist;
using Foundation.Features.Search.Search;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Settings
{
    [SettingsContentType(DisplayName = "Site Structure Settings Page",
        GUID = "bf69f959-c91b-46cb-9829-2ecf9d11e13b",
        Description = "Site structure settings",
        SettingsName = "Page references")]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-structure-settings.png")]
    public class ReferencePageSettings : SettingsBase
    {
        #region Site Structure

        [CultureSpecific]
        [AllowedTypes(typeof(SearchResultPage))]
        [Display(Name = "Search page", GroupName = TabNames.SiteStructure, Order = 10)]
        public virtual ContentReference SearchPage { get; set; }

        [CultureSpecific]
        [Display(Name = "Store locator page", GroupName = TabNames.SiteStructure, Order = 20)]
        public virtual ContentReference StoreLocatorPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(AddressBookPage))]
        [Display(Name = "Address book page", GroupName = TabNames.SiteStructure, Order = 30)]
        public virtual ContentReference AddressBookPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(ResetPasswordPage))]
        [Display(Name = "Reset password page", GroupName = TabNames.SiteStructure, Order = 40)]
        public virtual ContentReference ResetPasswordPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(WishListPage))]
        [Display(Name = "Wishlist page", GroupName = TabNames.SiteStructure, Order = 50)]
        public virtual ContentReference WishlistPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(CartPage))]
        [Display(Name = "Shopping cart page", GroupName = TabNames.SiteStructure, Order = 60)]
        public virtual ContentReference CartPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(SharedCartPage))]
        [Display(Name = "Shared cart page", GroupName = TabNames.SiteStructure, Order = 70)]
        public virtual ContentReference SharedCartPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(SubscriptionDetailPage))]
        [Display(Name = "Payment plan details page", GroupName = TabNames.SiteStructure, Order = 80)]
        public virtual ContentReference PaymentPlanDetailsPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(SubscriptionHistoryPage))]
        [Display(Name = "Payment plan history page", GroupName = TabNames.SiteStructure, Order = 90)]
        public virtual ContentReference PaymentPlanHistoryPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(OrganizationPage))]
        [Display(Name = "Organization main page", GroupName = TabNames.SiteStructure, Order = 100)]
        public virtual ContentReference OrganizationMainPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(SubOrganizationPage))]
        [Display(Name = "Sub-organization page", GroupName = TabNames.SiteStructure, Order = 110)]
        public virtual ContentReference SubOrganizationPage { get; set; }

        [CultureSpecific]
        [Display(Name = "Organization order pads page", GroupName = TabNames.SiteStructure, Order = 120)]
        [AllowedTypes(typeof(OrderPadsPage))]
        public virtual ContentReference OrganizationOrderPadsPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(QuickOrderPage))]
        [Display(Name = "Quick order page", GroupName = TabNames.SiteStructure, Order = 130)]
        public virtual ContentReference QuickOrderPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(OrderDetailsPage))]
        [Display(Name = "Order details page", GroupName = TabNames.SiteStructure, Order = 140)]
        public virtual ContentReference OrderDetailsPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(OrderHistoryPage))]
        [Display(Name = "Order history page", GroupName = TabNames.SiteStructure, Order = 150)]
        public virtual ContentReference OrderHistoryPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(OrderConfirmationPage))]
        [Display(Name = "Order confirmation page", GroupName = TabNames.SiteStructure, Order = 160)]
        public virtual ContentReference OrderConfirmationPage { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(CheckoutPage))]
        [Display(Name = "Checkout page", GroupName = TabNames.SiteStructure, Order = 170)]
        public virtual ContentReference CheckoutPage { get; set; }

        [CultureSpecific]
        [Display(Name = "Resource not found page", GroupName = TabNames.SiteStructure, Order = 180)]
        public virtual ContentReference PageNotFound { get; set; }

        #endregion

        #region Mail templates

        [CultureSpecific]
        [Display(Name = "Send order confirmations", GroupName = TabNames.MailTemplates, Order = 10)]
        public virtual bool SendOrderConfirmationMail { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(OrderConfirmationMailPage))]
        [Display(Name = "Order confirmation", GroupName = TabNames.MailTemplates, Order = 20)]
        public virtual ContentReference OrderConfirmationMail { get; set; }

        [CultureSpecific]
        [AllowedTypes(typeof(ResetPasswordMailPage))]
        [Display(Name = "Reset password", GroupName = TabNames.MailTemplates, Order = 30)]
        public virtual ContentReference ResetPasswordMail { get; set; }

        #endregion
    }
}