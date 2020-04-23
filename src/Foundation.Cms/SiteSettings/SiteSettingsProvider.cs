using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Cache;
using Foundation.Cms.Pages;
using Foundation.Cms.ViewModels;
using System;
using System.Collections.Generic;

namespace Foundation.Cms.SiteSettings
{
    public class SiteSettingsProvider : ISiteSettingsProvider
    {
        private readonly IContentLoader _contentLoader;
        private readonly IContentCacheKeyCreator _contentCacheKeyCreator;

        public SiteSettingsProvider(IContentLoader contentLoader,
            IContentCacheKeyCreator contentCacheKeyCreator)
        {
            _contentLoader = contentLoader;
            _contentCacheKeyCreator = contentCacheKeyCreator;
        }

        public T GetSettingProperty<T>(string propertyName, CmsSettingsPage settingsPage) => (T)_GetProperty(propertyName, settingsPage);
        public T GetSettingProperty<T>(string propertyName, CmsHomePage homePage)
        {
            if (ContentReference.IsNullOrEmpty(homePage.SettingsPage))
            {
                return default(T);
            }

            var settingsPage = _contentLoader.Get<CmsSettingsPage>(homePage.SettingsPage);
            return (T)_GetProperty(propertyName, settingsPage);
        }

        public T GetSiteSettings<T>(CmsHomePage homePage) where T : CmsSiteSettings, new()
        {
            var siteSettings = CacheManager.Get(_GetCacheKey(homePage)) as CmsSiteSettings;
            if (siteSettings == null)
            {
                if (ContentReference.IsNullOrEmpty(homePage.SettingsPage))
                {
                    return new T();
                }

                var settingsPage = _contentLoader.Get<CmsSettingsPage>(homePage.SettingsPage);
                var ancestorsSettingsPage = _contentLoader.GetAncestors(settingsPage.ContentLink);
                var settings = _GetSettings<T>(settingsPage);
                var keyDependency = new List<string>();
                keyDependency.Add(_contentCacheKeyCreator.CreateCommonCacheKey(settingsPage.ContentLink));
                keyDependency.Add(_contentCacheKeyCreator.CreateCommonCacheKey(homePage.ContentLink));

                foreach (var anc in ancestorsSettingsPage)
                {
                    keyDependency.Add(_contentCacheKeyCreator.CreateCommonCacheKey(anc.ContentLink));
                }


                var eviction = new CacheEvictionPolicy(TimeSpan.FromDays(1), CacheTimeoutType.Sliding, keyDependency);
                CacheManager.Insert(_GetCacheKey(homePage), settings, eviction);

                return settings;
            }

            return (T)siteSettings;
        }

        private object _GetProperty(string propertyName, CmsSettingsPage settingsPage)
        {
            if (settingsPage == null)
            {
                return null;
            }

            var result = settingsPage.GetType().GetProperty(propertyName).GetValue(settingsPage);
            if (result != null || settingsPage.ParentLink == ContentReference.RootPage)
            {
                return result;
            }

            var parentSettingPage = _contentLoader.Get<PageData>(settingsPage.ParentLink) as CmsSettingsPage;
            return _GetProperty(propertyName, parentSettingPage);
        }

        private T _GetSettings<T>(CmsSettingsPage settingsPage) where T : CmsSiteSettings, new()
        {
            var model = new T();
            if (settingsPage == null)
            {
                return model;
            }

            var properties = model.GetType().GetProperties();
            foreach (var p in properties)
            {
                var pValue = _GetProperty(p.Name, settingsPage);
                model.GetType().GetProperty(p.Name).SetValue(model, pValue);
            }

            return model;
        }

        private string _GetCacheKey(CmsHomePage homePage) => $"SiteSettings:{homePage.ContentLink.ID}:{homePage.Language.DisplayName}";
    }
}
