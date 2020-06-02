using Foundation.Cms.SiteSettings.Interfaces;
using Foundation.Cms.SiteSettings.Models;
using Foundation.Commerce.SiteSettings.Interfaces;

namespace Foundation.Commerce.SiteSettings.Models
{
    public class CommerceSettingsModel : ICommerceSettingsModel
    {
        public CommerceSettingsModel()
        {
            LayoutSettings = new CommerceLayoutSettingsModel();
            SiteStructureSettings = new SiteStructureSettingsModel();
            SelectionSettings = new SelectionSettingsModel();
        }

        public ICmsLayoutSettings LayoutSettings { get; set; }
        public ISiteStructureSettings SiteStructureSettings{ get; set; }
        public ISelectionSettings SelectionSettings { get; set; }
        public ICommerceLayoutSettings CommerceLayoutSettings => LayoutSettings as ICommerceLayoutSettings;
    }
}
