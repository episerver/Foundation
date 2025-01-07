using System.Collections.Concurrent;

namespace Foundation.Infrastructure.Cms.Settings;

public interface ISettingsService
{
    ContentReference GlobalSettingsRoot { get; set; }
    ConcurrentDictionary<Guid, Dictionary<Type, Guid>> SiteSettings { get; }
    T GetSiteSettings<T>(Guid? siteId = null, string language = null) where T : SettingsBase;
    void InitializeSettings();
    void RegisterContentRoots();
    void UpdateSettings(Guid siteId, IContent content);
    void UpdateSettings();
}
