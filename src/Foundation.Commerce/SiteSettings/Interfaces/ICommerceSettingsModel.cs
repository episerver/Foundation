using Foundation.Cms.SiteSettings.Interfaces;

namespace Foundation.Commerce.SiteSettings.Interfaces
{
    public interface ICommerceSettingsModel : ICmsSettingsModel
    {
        ISiteStructureSettings SiteStructureSettings { get; set; }
    }
}
