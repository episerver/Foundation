using Foundation.Cms.SiteSettings.Interfaces;
using Foundation.Cms.SiteSettings.Pages;
using System.Collections.Generic;

namespace Foundation.Cms.SiteSettings.Implementations
{
    public class CmsSettingsHandler : ISettingsHandler
    {
        public virtual T CreateSettingsModel<T>(IEnumerable<SettingsBasePage> settingsPages) where T : ICmsSettingsModel, new()
        {
            var model = new T();
            foreach(var s in settingsPages)
            {
                if (s is ICmsLayoutSettings)
                {
                    model.LayoutSettings = s as ICmsLayoutSettings;
                }

                if (s is ISelectionSettings)
                {
                    model.SelectionSettings = s as ISelectionSettings;
                }
            }

            return model;
        }
    }
}
