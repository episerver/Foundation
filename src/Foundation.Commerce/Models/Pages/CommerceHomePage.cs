using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using Foundation.Cms.Pages;
using Foundation.Cms.SiteSettings;
using Foundation.Cms.ViewModels;
using Foundation.Commerce.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Commerce.Models.Pages
{
    public abstract class CommerceHomePage : CmsHomePage
    {
        [CultureSpecific]
        [Display(Name = "Settings page", GroupName = CommerceTabNames.SiteStructure, Order = 100)]
        [AllowedTypes(new[] { typeof(CommerceSettingsPage) })]
        public override ContentReference SettingsPage { get; set; }

        // Get settings
        public override CmsSiteSettings SiteSettings
        {
            get
            {
                var _siteSettingsProvider = ServiceLocator.Current.GetInstance<ISiteSettingsProvider>();
                return _siteSettingsProvider.GetSiteSettings<CommerceSiteSettings>(this);
            }
        }

        public virtual XhtmlString BannerText => (SiteSettings as CommerceSiteSettings).BannerText;
        public virtual string MyAccountLabel => (SiteSettings as CommerceSiteSettings).MyAccountLabel;
        public virtual string CartLabel => (SiteSettings as CommerceSiteSettings).CartLabel;
        public virtual string SearchLabel => (SiteSettings as CommerceSiteSettings).SearchLabel;
        public virtual string WishlistLabel => (SiteSettings as CommerceSiteSettings).WishlistLabel;
        public virtual string SharedCartLabel => (SiteSettings as CommerceSiteSettings).SharedCartLabel;
        public virtual LinkItemCollection MyAccountCommerceMenu => (SiteSettings as CommerceSiteSettings).MyAccountCommerceMenu;
        public virtual LinkItemCollection OrganizationMenu => (SiteSettings as CommerceSiteSettings).OrganizationMenu;
        public virtual bool ShowProductRatingsOnListings => (SiteSettings as CommerceSiteSettings).ShowProductRatingsOnListings;
        public virtual ContentReference SearchPage => (SiteSettings as CommerceSiteSettings).SearchPage;
        public virtual ContentReference StoreLocatorPage => (SiteSettings as CommerceSiteSettings).StoreLocatorPage;
        public virtual ContentReference AddressBookPage => (SiteSettings as CommerceSiteSettings).AddressBookPage;
        public virtual ContentReference ResetPasswordPage => (SiteSettings as CommerceSiteSettings).ResetPasswordPage;
        public virtual ContentReference WishlistPage => (SiteSettings as CommerceSiteSettings).WishlistPage;
        public virtual ContentReference CartPage => (SiteSettings as CommerceSiteSettings).CartPage;
        public virtual ContentReference SharedCartPage => (SiteSettings as CommerceSiteSettings).SharedCartPage;
        public virtual ContentReference PaymentPlanDetailsPage => (SiteSettings as CommerceSiteSettings).PaymentPlanDetailsPage;
        public virtual ContentReference PaymentPlanHistoryPage => (SiteSettings as CommerceSiteSettings).PaymentPlanHistoryPage;
        public virtual ContentReference OrganizationMainPage => (SiteSettings as CommerceSiteSettings).OrganizationMainPage;
        public virtual ContentReference SubOrganizationPage => (SiteSettings as CommerceSiteSettings).SubOrganizationPage;
        public virtual ContentReference OrganizationOrderPadsPage => (SiteSettings as CommerceSiteSettings).OrganizationOrderPadsPage;
        public virtual ContentReference QuickOrderPage => (SiteSettings as CommerceSiteSettings).QuickOrderPage;
        public virtual ContentReference OrderDetailsPage => (SiteSettings as CommerceSiteSettings).OrderDetailsPage;
        public virtual ContentReference OrderHistoryPage => (SiteSettings as CommerceSiteSettings).OrderHistoryPage;
        public virtual ContentReference OrderConfirmationPage => (SiteSettings as CommerceSiteSettings).OrderConfirmationPage;
        public virtual ContentReference CheckoutPage => (SiteSettings as CommerceSiteSettings).CheckoutPage;
        public virtual ContentReference PageNotFound => (SiteSettings as CommerceSiteSettings).PageNotFound;
        public virtual bool SendOrderConfirmationMail => (SiteSettings as CommerceSiteSettings).SendOrderConfirmationMail;
        public virtual ContentReference OrderConfirmationMail => (SiteSettings as CommerceSiteSettings).OrderConfirmationMail;
        public virtual ContentReference ResetPasswordMail => (SiteSettings as CommerceSiteSettings).ResetPasswordMail;
    }
}