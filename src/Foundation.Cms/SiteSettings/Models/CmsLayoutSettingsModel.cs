using EPiServer.Core;
using EPiServer.SpecializedProperties;
using Foundation.Cms.SiteSettings.Interfaces;

namespace Foundation.Cms.SiteSettings.Models
{
    public class CmsLayoutSettingsModel : ICmsLayoutSettings
    {
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
