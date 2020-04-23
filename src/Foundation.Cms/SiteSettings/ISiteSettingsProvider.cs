using Foundation.Cms.Pages;
using Foundation.Cms.ViewModels;

namespace Foundation.Cms.SiteSettings
{
    public interface ISiteSettingsProvider
    {
        /// <summary>
        /// Get setting property by name
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="settingsPage">SettingsPage</param>
        /// <returns></returns>
        T GetSettingProperty<T>(string propertyName, CmsSettingsPage settingsPage);

        /// <summary>
        /// Get setting property by name
        /// </summary>
        /// <typeparam name="T">Type of the property</typeparam>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="homePage">CmsHomePage</param>
        /// <returns></returns>
        T GetSettingProperty<T>(string propertyName, CmsHomePage homePage);

        /// <summary>
        /// Get settings of home page from its settings page
        /// </summary>
        /// <typeparam name="T">Type of site settings (CmsSiteSettings)</typeparam>
        /// <param name="homePage">CmsHomePage</param>
        /// <returns></returns>
        T GetSiteSettings<T>(CmsHomePage homePage) where T : CmsSiteSettings, new();
    }
}
