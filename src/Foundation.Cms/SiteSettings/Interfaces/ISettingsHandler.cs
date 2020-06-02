using Foundation.Cms.SiteSettings.Pages;
using System.Collections.Generic;

namespace Foundation.Cms.SiteSettings.Interfaces
{
    public interface ISettingsHandler
    {
        T CreateSettingsModel<T>(IEnumerable<SettingsBasePage> settingsPages) where T : ICmsSettingsModel, new();
    }
}
