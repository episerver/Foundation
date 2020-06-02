using Foundation.Cms.SiteSettings.Interfaces;
using Foundation.Cms.SiteSettings.Models;
using Foundation.Commerce.SiteSettings.Interfaces;
using Foundation.Commerce.SiteSettings.Models;
using Foundation.Demo.SiteSettings.Interfaces;

namespace Foundation.Demo.SiteSettings.Models
{
    public class DemoSettingsModel : IDemoSettingsModel
    {
        public DemoSettingsModel()
        {
            LayoutSettings = new DemoLayoutSettingsModel();
            SiteStructureSettings = new SiteStructureSettingsModel();
            SearchSettings = new DemoSearchSettingsModel();
            SelectionSettings = new SelectionSettingsModel();
        }

        public ICmsLayoutSettings LayoutSettings { get; set; }
        public ISiteStructureSettings SiteStructureSettings { get; set; }
        public ISelectionSettings SelectionSettings { get; set; }
        public IDemoSearchSettings SearchSettings { get; set; }
        public IDemoLayoutSettings DemoLayoutSettings => LayoutSettings as IDemoLayoutSettings;
    }
}
