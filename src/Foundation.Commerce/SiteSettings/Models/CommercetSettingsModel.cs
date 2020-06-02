using Foundation.Cms.SiteSettings.Interfaces;
using Foundation.Commerce.SiteSettings.Interfaces;

namespace Foundation.Commerce.SiteSettings.Models
{
    public class CommerceSettingsModel : ICommerceSettingsModel
    {
        public CommerceSettingsModel()
        {
            LayoutSettings = new CommerceLayoutSettingsModel();
            SiteStructureSettings = new SiteStructureSettingsModel();
        }

        public ICmsLayoutSettings LayoutSettings { get; set; }
        public ISiteStructureSettings SiteStructureSettings{ get; set; }

        public ICommerceLayoutSettings CommerceLayoutSettings => LayoutSettings as ICommerceLayoutSettings;
    }
}
