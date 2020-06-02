using EPiServer.Core;
using EPiServer.SpecializedProperties;
using Foundation.Demo.SiteSettings.Interfaces;

namespace Foundation.Demo.SiteSettings.Models
{
    public class DemoLayoutSettingsModel : IDemoLayoutSettings
    {
        public ContentReference SiteLogo { get; set; }
        public string HeaderMenuStyle { get; set; }
        public bool LargeHeaderMenu { get; set; }
        public bool ShowCommerceHeaderComponents { get; set; }
        public bool StickyTopHeader { get; set; }
        public XhtmlString BannerText { get; set; }
        public LinkItemCollection MyAccountCommerceMenu { get; set; }
        public LinkItemCollection OrganizationMenu { get; set; }
        public string MyAccountLabel { get; set; }
        public string CartLabel { get; set; }
        public string SearchLabel { get; set; }
        public string WishlistLabel { get; set; }
        public string SharedCartLabel { get; set; }
        public string Introduction { get; set; }
        public string CompanyHeader { get; set; }
        public string CompanyName { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        public string LinksHeader { get; set; }
        public LinkItemCollection Links { get; set; }
        public string SocialHeader { get; set; }
        public LinkItemCollection SocialLinks { get; set; }
        public ContentArea ContentArea { get; set; }
        public string FooterCopyrightText { get; set; }
        public ContentArea MainMenu { get; set; }
        public LinkItemCollection MyAccountCmsMenu { get; set; }
    }
}
