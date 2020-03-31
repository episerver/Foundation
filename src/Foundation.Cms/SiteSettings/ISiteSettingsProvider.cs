using Foundation.Cms.Pages;
using Foundation.Cms.ViewModels;

namespace Foundation.Cms.SiteSettings
{
    public interface ISiteSettingsProvider
    {
        T GetSettingProperty<T>(string propertyName, CmsSettingsPage settingsPage);
        T GetSiteSettings<T>(CmsHomePage homePage) where T : CmsSiteSettings, new();
    }
}
