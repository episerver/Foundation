using AddOn.Episerver.Settings.Core;
using EPiServer;
using EPiServer.Core;
using Foundation.Cms.SiteSettings.Interfaces;
using Foundation.Cms.SiteSettings.Pages;

namespace Foundation.Cms.SiteSettings.Implementations
{
    public class SiteSettingsProvider : ISiteSettingsProvider
    {
        private readonly ISettingsService _settingsService;
        private readonly IContentLoader _contentLoader;
        private readonly ISettingsHandler _settingsHandler;
        public SiteSettingsProvider(ISettingsService settingsService,
            IContentLoader contentLoader,
            ISettingsHandler settingsHandler)
        {
            _settingsService = settingsService;
            _contentLoader = contentLoader;
            _settingsHandler = settingsHandler;
        }

        public T GetSettings<T>(SettingNode settingNode) where T : SettingsBasePage
        {
            return _settingsService.GetSettings<T>(settingNode);
        }

        public T GetSiteSettings<T>(ContentReference settingNodeRef) where T : ICmsSettingsModel, new()
        {
            if (ContentReference.IsNullOrEmpty(settingNodeRef))
            {
                return new T();
            }

            var settingsPages = _contentLoader.GetChildren<SettingsBasePage>(settingNodeRef);
            return _settingsHandler.CreateSettingsModel<T>(settingsPages);
        }
    }
}
