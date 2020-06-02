using Foundation.Cms.SiteSettings.Interfaces;

namespace Foundation.Cms.SiteSettings.Models
{
    public class CmsSettingsModel : ICmsSettingsModel
    {
        public CmsSettingsModel()
        {
            LayoutSettings = new CmsLayoutSettingsModel();
            SelectionSettings = new SelectionSettingsModel();
        }

        public ICmsLayoutSettings LayoutSettings { get; set; }
        public ISelectionSettings SelectionSettings { get; set; }
    }
}
