using EPiServer.Core;
using Foundation.Cms.SiteSettings.Pages;

namespace Foundation.Cms.SiteSettings.Interfaces
{
    public interface ISiteSettingsProvider
    {
        T GetSettings<T>(SettingNode settingNode) where T : SettingsBasePage;

        /// <summary>
        /// Get all settings
        /// </summary>
        /// <typeparam name="T">Settings Model</typeparam>
        /// <param name="settingNodeRef">Setting Node ContentLink</param>
        /// <returns></returns>
        T GetSiteSettings<T>(ContentReference settingNodeRef) where T : ICmsSettingsModel, new();
    }
}
