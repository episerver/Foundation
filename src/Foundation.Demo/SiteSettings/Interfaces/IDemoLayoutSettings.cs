using EPiServer.Core;
using Foundation.Commerce.SiteSettings.Interfaces;

namespace Foundation.Demo.SiteSettings.Interfaces
{
    public interface IDemoLayoutSettings : ICommerceLayoutSettings
    { 
        ContentReference SiteLogo { get; set; }
        string HeaderMenuStyle { get; set; }
        bool LargeHeaderMenu { get; set; }
        bool ShowCommerceHeaderComponents { get; set; }
        bool StickyTopHeader { get; set; }
    }
}
