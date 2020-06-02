using EPiServer.Core;
using EPiServer.SpecializedProperties;
using Foundation.Cms.SiteSettings.Interfaces;

namespace Foundation.Commerce.SiteSettings.Interfaces
{
    public interface ICommerceLayoutSettings : ICmsLayoutSettings
    { 
        XhtmlString BannerText { get; set; }
        LinkItemCollection MyAccountCommerceMenu { get; set; }
        LinkItemCollection OrganizationMenu { get; set; }
        string MyAccountLabel { get; set; }
        string CartLabel { get; set; }
        string SearchLabel { get; set; }
        string WishlistLabel { get; set; }
        string SharedCartLabel { get; set; }
    }
}
