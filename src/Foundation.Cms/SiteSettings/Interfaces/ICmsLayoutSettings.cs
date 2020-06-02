using EPiServer.Core;
using EPiServer.SpecializedProperties;

namespace Foundation.Cms.SiteSettings.Interfaces
{
    public interface ICmsLayoutSettings { 
        string Introduction { get; set; }
        string CompanyHeader { get; set; }
        string CompanyName { get; set; }
        string CompanyAddress { get; set; }
        string CompanyPhone { get; set; }
        string CompanyEmail { get; set; }
        string LinksHeader { get; set; }
        LinkItemCollection Links { get; set; }
        string SocialHeader { get; set; }
        LinkItemCollection SocialLinks { get; set; }
        ContentArea ContentArea { get; set; }
        string FooterCopyrightText { get; set; }
        ContentArea MainMenu { get; set; }
        LinkItemCollection MyAccountCmsMenu { get; set; }
    }
}
