using Foundation.Commerce.SiteSettings.Interfaces;

namespace Foundation.Demo.SiteSettings.Interfaces
{
    public interface IDemoSettingsModel : ICommerceSettingsModel
    {
        IDemoSearchSettings SearchSettings { get; set; }
    }
}
