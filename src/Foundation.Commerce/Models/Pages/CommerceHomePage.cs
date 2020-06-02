using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using Foundation.Cms.Pages;
using Foundation.Cms.SiteSettings.Interfaces;
using Foundation.Commerce.SiteSettings.Models;

namespace Foundation.Commerce.Models.Pages
{
    public abstract class CommerceHomePage : CmsHomePage
    {
        #region Settings properties
        private CommerceSettingsModel CommerceSiteSettings
        {
            get
            {
                var siteSettingsProvider = ServiceLocator.Current.GetInstance<ISiteSettingsProvider>();
                var settings = siteSettingsProvider.GetSiteSettings<CommerceSettingsModel>(this.SettingNode);
                return settings;
            }
        }

        public XhtmlString BannerText => CommerceSiteSettings.CommerceLayoutSettings.BannerText;
        public LinkItemCollection MyAccountCommerceMenu => CommerceSiteSettings.CommerceLayoutSettings.MyAccountCommerceMenu;
        public LinkItemCollection OrganizationMenu => CommerceSiteSettings.CommerceLayoutSettings.OrganizationMenu;
        public string MyAccountLabel => CommerceSiteSettings.CommerceLayoutSettings.MyAccountLabel;
        public string CartLabel => CommerceSiteSettings.CommerceLayoutSettings.CartLabel;
        public string SearchLabel => CommerceSiteSettings.CommerceLayoutSettings.SearchLabel;
        public string WishlistLabel => CommerceSiteSettings.CommerceLayoutSettings.WishlistLabel;
        public string SharedCartLabel => CommerceSiteSettings.CommerceLayoutSettings.SharedCartLabel;

        public bool ShowProductRatingsOnListings => CommerceSiteSettings.SiteStructureSettings.ShowProductRatingsOnListings;
        public ContentReference SearchPage => CommerceSiteSettings.SiteStructureSettings.SearchPage;
        public ContentReference StoreLocatorPage => CommerceSiteSettings.SiteStructureSettings.StoreLocatorPage;
        public ContentReference AddressBookPage => CommerceSiteSettings.SiteStructureSettings.AddressBookPage;
        public ContentReference ResetPasswordPage => CommerceSiteSettings.SiteStructureSettings.ResetPasswordPage;
        public ContentReference WishlistPage => CommerceSiteSettings.SiteStructureSettings.WishlistPage;
        public ContentReference CartPage => CommerceSiteSettings.SiteStructureSettings.CartPage;
        public ContentReference SharedCartPage => CommerceSiteSettings.SiteStructureSettings.SharedCartPage;
        public ContentReference PaymentPlanDetailsPage => CommerceSiteSettings.SiteStructureSettings.PaymentPlanDetailsPage;
        public ContentReference PaymentPlanHistoryPage => CommerceSiteSettings.SiteStructureSettings.PaymentPlanHistoryPage;
        public ContentReference OrganizationMainPage => CommerceSiteSettings.SiteStructureSettings.OrganizationMainPage;
        public ContentReference SubOrganizationPage => CommerceSiteSettings.SiteStructureSettings.SubOrganizationPage;
        public ContentReference OrganizationOrderPadsPage => CommerceSiteSettings.SiteStructureSettings.OrganizationOrderPadsPage;
        public ContentReference QuickOrderPage => CommerceSiteSettings.SiteStructureSettings.QuickOrderPage;
        public ContentReference OrderDetailsPage => CommerceSiteSettings.SiteStructureSettings.OrderDetailsPage;
        public ContentReference OrderHistoryPage => CommerceSiteSettings.SiteStructureSettings.OrderHistoryPage;
        public ContentReference OrderConfirmationPage => CommerceSiteSettings.SiteStructureSettings.OrderConfirmationPage;
        public ContentReference CheckoutPage => CommerceSiteSettings.SiteStructureSettings.CheckoutPage;
        public ContentReference PageNotFound => CommerceSiteSettings.SiteStructureSettings.PageNotFound;
        public bool SendOrderConfirmationMail => CommerceSiteSettings.SiteStructureSettings.SendOrderConfirmationMail;
        public ContentReference OrderConfirmationMail => CommerceSiteSettings.SiteStructureSettings.OrderConfirmationMail;
        public ContentReference ResetPasswordMail => CommerceSiteSettings.SiteStructureSettings.ResetPasswordMail;
        #endregion
    }
}