using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAccess;
using EPiServer.Framework.TypeScanner;
using EPiServer.Globalization;
using EPiServer.Logging;
using EPiServer.Security;
using EPiServer.Web;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Infrastructure.Cms.Settings
{
    public interface ISettingsService
    {
        ContentReference GlobalSettingsRoot { get; set; }
        ConcurrentDictionary<string, Dictionary<Type, object>> SiteSettings { get; }
        T GetSiteSettings<T>(Guid? siteId = null);
        void InitializeSettings();
        void UnintializeSettings();
        void UpdateSettings(Guid siteId, IContent content, bool isContentNotPublished);
        void UpdateSettings();
    }

    public static class ISettingsServiceExtensions
    {
        public static T GetSiteSettingsOrThrow<T>(this ISettingsService settingsService,
            Func<T, bool> shouldThrow,
            string message) where T : SettingsBase
        {
            var settings = settingsService.GetSiteSettings<T>();
            if (settings == null || (shouldThrow?.Invoke(settings) ?? false))
            {
                throw new InvalidOperationException(message);
            }

            return settings;
        }

        public static bool TryGetSiteSettings<T>(this ISettingsService settingsService, out T value) where T : SettingsBase
        {
            value = settingsService.GetSiteSettings<T>();
            return value != null;
        }
    }

    public class SettingsService : ISettingsService
    {
        public const string GlobalSettingsRootName = "Global Settings Root";
        private readonly IContentRepository _contentRepository;
        private readonly IContentVersionRepository _contentVersionRepository;
        private readonly ContentRootService _contentRootService;
        private readonly IContentTypeRepository _contentTypeRepository;
        private readonly ILogger _log = LogManager.GetLogger();
        private readonly ITypeScannerLookup _typeScannerLookup;
        private readonly IContentEvents _contentEvents;
        private readonly ISiteDefinitionEvents _siteDefinitionEvents;
        private readonly ISiteDefinitionRepository _siteDefinitionRepository;
        private readonly ISiteDefinitionResolver _siteDefinitionResolver;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IContextModeResolver _contextModeResolver;

        public SettingsService(
            IContentRepository contentRepository,
            IContentVersionRepository contentVersionRepository,
            ContentRootService contentRootService,
            ITypeScannerLookup typeScannerLookup,
            IContentTypeRepository contentTypeRepository,
            IContentEvents contentEvents,
            ISiteDefinitionEvents siteDefinitionEvents,
            ISiteDefinitionRepository siteDefinitionRepository,
            ISiteDefinitionResolver siteDefinitionResolver,
            IHttpContextAccessor httpContextAccessor,
            IContextModeResolver contextModeResolver)
        {
            _contentRepository = contentRepository;
            _contentVersionRepository = contentVersionRepository;
            _contentRootService = contentRootService;
            _typeScannerLookup = typeScannerLookup;
            _contentTypeRepository = contentTypeRepository;
            _contentEvents = contentEvents;
            _siteDefinitionEvents = siteDefinitionEvents;
            _siteDefinitionRepository = siteDefinitionRepository;
            _siteDefinitionResolver = siteDefinitionResolver;
            _httpContextAccessor = httpContextAccessor;
            _contextModeResolver = contextModeResolver;
        }

        public ConcurrentDictionary<string, Dictionary<Type, object>> SiteSettings { get; } = new ConcurrentDictionary<string, Dictionary<Type, object>>();

        public ContentReference GlobalSettingsRoot { get; set; }

        public T GetSiteSettings<T>(Guid? siteId = null)
        {
            var contentLanguage = ContentLanguage.PreferredCulture.Name;
            if (!siteId.HasValue)
            {
                siteId = ResolveSiteId();
                if (siteId == Guid.Empty)
                {
                    return default;
                }
            }
            try
            {
                if (_contextModeResolver.CurrentMode == ContextMode.Edit)
                {
                    if (SiteSettings.TryGetValue(siteId.Value.ToString() + $"-common-draft-{contentLanguage}", out var siteSettings))
                    {
                        if (siteSettings.TryGetValue(typeof(T), out var setting))
                        {
                            return (T)setting;
                        }
                    }
                    if (SiteSettings.TryGetValue(siteId.Value.ToString() + "-common-draft-default", out var defaultSiteSettings))
                    {
                        if (defaultSiteSettings.TryGetValue(typeof(T), out var defaultSetting))
                        {
                            return (T)defaultSetting;
                        }
                    }
                }
                else
                {
                    if (SiteSettings.TryGetValue(siteId.Value.ToString() + $"-{contentLanguage}", out var siteSettings) && siteSettings.TryGetValue(typeof(T), out var setting))
                    {
                        return (T)setting;
                    }
                    if (SiteSettings.TryGetValue(siteId.Value.ToString() + "-default", out var defaultSiteSettings) && defaultSiteSettings.TryGetValue(typeof(T), out var defaultSetting))
                    {
                        return (T)defaultSetting;
                    }
                }
            }
            catch (KeyNotFoundException keyNotFoundException)
            {
                _log.Error($"[Settings] {keyNotFoundException.Message}", exception: keyNotFoundException);
            }
            catch (ArgumentNullException argumentNullException)
            {
                _log.Error($"[Settings] {argumentNullException.Message}", exception: argumentNullException);
            }

            return default;
        }

        public void UpdateSettings(Guid siteId, IContent content, bool isContentNotPublished)
        {
            var contentType = content.GetOriginalType();
            var contentLanguage = ContentLanguage.PreferredCulture.Name;
            try
            {
                if (isContentNotPublished)
                {
                    if (!SiteSettings.ContainsKey(siteId.ToString() + $"-default"))
                    {
                        SiteSettings[$"{siteId}-common-draft-default"] = new Dictionary<Type, object>();
                    }

                    if (!SiteSettings[$"{siteId}-common-draft-default"].ContainsKey(contentType))
                    {
                        SiteSettings[$"{siteId}-common-draft-default"][contentType] = content;
                    }

                    if (!SiteSettings.ContainsKey(siteId.ToString() + $"-{contentLanguage}"))
                    {
                        SiteSettings[$"{siteId}-common-draft-{contentLanguage}"] = new Dictionary<Type, object>();
                    }

                    SiteSettings[$"{siteId}-common-draft-{contentLanguage}"][contentType] = content;
                }
                else
                {
                    if (!SiteSettings.ContainsKey(siteId.ToString() + $"-default"))
                    {
                        SiteSettings[siteId.ToString() + $"-default"] = new Dictionary<Type, object>();
                        SiteSettings[$"{siteId}-common-draft-default"] = new Dictionary<Type, object>();
                    }

                    if (!SiteSettings[$"{siteId}-default"].ContainsKey(contentType))
                    {
                        SiteSettings[$"{siteId}-default"][contentType] = content;
                    }

                    if (!SiteSettings[$"{siteId}-common-draft-default"].ContainsKey(contentType))
                    {
                        SiteSettings[$"{siteId}-common-draft-default"][contentType] = content;
                    }

                    if (!SiteSettings.ContainsKey(siteId.ToString() + $"-{contentLanguage}"))
                    {
                        SiteSettings[siteId.ToString() + $"-{contentLanguage}"] = new Dictionary<Type, object>();
                        SiteSettings[$"{siteId}-common-draft-{contentLanguage}"] = new Dictionary<Type, object>();
                    }

                    SiteSettings[siteId.ToString() + $"-{contentLanguage}"][contentType] = content;
                    SiteSettings[$"{siteId}-common-draft-{contentLanguage}"][contentType] = content;
                }
            }
            catch (KeyNotFoundException keyNotFoundException)
            {
                _log.Error($"[Settings] {keyNotFoundException.Message}", exception: keyNotFoundException);
            }
            catch (ArgumentNullException argumentNullException)
            {
                _log.Error($"[Settings] {argumentNullException.Message}", exception: argumentNullException);
            }
        }

        public void InitializeSettings()
        {
            try
            {
                RegisterContentRoots();
            }
            catch (NotSupportedException notSupportedException)
            {
                _log.Error($"[Settings] {notSupportedException.Message}", exception: notSupportedException);
                throw;
            }

            _contentEvents.PublishedContent += PublishedContent;
            _contentEvents.SavedContent += SavedContent;
            _siteDefinitionEvents.SiteCreated += SiteCreated;
            _siteDefinitionEvents.SiteUpdated += SiteUpdated;
            _siteDefinitionEvents.SiteDeleted += SiteDeleted;
        }

        public void UnintializeSettings()
        {
            _contentEvents.PublishedContent -= PublishedContent;
            _contentEvents.SavedContent -= SavedContent;
            _siteDefinitionEvents.SiteCreated -= SiteCreated;
            _siteDefinitionEvents.SiteUpdated -= SiteUpdated;
            _siteDefinitionEvents.SiteDeleted -= SiteDeleted;
        }

        public void UpdateSettings()
        {
            var root = _contentRepository.GetItems(_contentRootService.List(), new LoaderOptions())
                 .FirstOrDefault(x => x.ContentGuid == SettingsFolder.SettingsRootGuid);

            if (root == null)
            {
                return;
            }

            GlobalSettingsRoot = root.ContentLink;
            var children = _contentRepository.GetChildren<SettingsFolder>(GlobalSettingsRoot).ToList();
            foreach (var site in _siteDefinitionRepository.List())
            {
                var folder = children.Find(x => x.Name.Equals(site.Name, StringComparison.InvariantCultureIgnoreCase));
                if (folder != null)
                {
                    foreach (var child in _contentRepository.GetChildren<SettingsBase>(folder.ContentLink))
                    {
                        UpdateSettings(site.Id, child, false);

                        // add draft (not published version) settings
                        var darftContentLink = _contentVersionRepository.LoadCommonDraft(child.ContentLink, ContentLanguage.PreferredCulture.Name);
                        if (darftContentLink != null)
                        {
                            var settingsDraft = _contentRepository.Get<SettingsBase>(darftContentLink.ContentLink);
                            UpdateSettings(site.Id, settingsDraft, true);
                        }
                    }
                    continue;
                }
                CreateSiteFolder(site);
            }
        }

        private void RegisterContentRoots()
        {
            var registeredRoots = _contentRepository.GetItems(_contentRootService.List(), new LoaderOptions());
            var settingsRootRegistered = registeredRoots.Any(x => x.ContentGuid == SettingsFolder.SettingsRootGuid && x.Name.Equals(SettingsFolder.SettingsRootName));

            if (!settingsRootRegistered)
            {
                _contentRootService.Register<SettingsFolder>(SettingsFolder.SettingsRootName, SettingsFolder.SettingsRootGuid, ContentReference.RootPage);
            }

            UpdateSettings();
        }

        private void CreateSiteFolder(SiteDefinition siteDefinition)
        {
            var folder = _contentRepository.GetDefault<SettingsFolder>(GlobalSettingsRoot);
            folder.Name = siteDefinition.Name;
            var reference = _contentRepository.Save(folder, SaveAction.Publish, AccessLevel.NoAccess);

            var settingsModelTypes = _typeScannerLookup.AllTypes
                .Where(t => t.GetCustomAttributes(typeof(SettingsContentTypeAttribute), false).Length > 0);

            foreach (var settingsType in settingsModelTypes)
            {
                if (!(settingsType.GetCustomAttributes(typeof(SettingsContentTypeAttribute), false)
                    .FirstOrDefault() is SettingsContentTypeAttribute attribute))
                {
                    continue;
                }

                var contentType = _contentTypeRepository.Load(settingsType);
                var newSettings = _contentRepository.GetDefault<IContent>(reference, contentType.ID);
                newSettings.Name = attribute.SettingsName;
                _contentRepository.Save(newSettings, SaveAction.Publish, AccessLevel.NoAccess);
                UpdateSettings(siteDefinition.Id, newSettings, false);
            }
        }

        private void SiteCreated(object sender, SiteDefinitionEventArgs e)
        {
            if (_contentRepository.GetChildren<SettingsFolder>(GlobalSettingsRoot)
                .Any(x => x.Name.Equals(e.Site.Name, StringComparison.InvariantCultureIgnoreCase)))
            {
                return;
            }

            CreateSiteFolder(e.Site);
        }

        private void SiteDeleted(object sender, SiteDefinitionEventArgs e)
        {
            var folder = _contentRepository.GetChildren<SettingsFolder>(GlobalSettingsRoot)
                .FirstOrDefault(x => x.Name.Equals(e.Site.Name, StringComparison.InvariantCultureIgnoreCase));

            if (folder == null)
            {
                return;
            }

            _contentRepository.Delete(folder.ContentLink, true, AccessLevel.NoAccess);
        }

        private void SiteUpdated(object sender, SiteDefinitionEventArgs e)
        {
            var updatedArgs = e as SiteDefinitionUpdatedEventArgs;
            var prevSite = updatedArgs.PreviousSite;
            var updatedSite = updatedArgs.Site;
            var settingsRoot = GlobalSettingsRoot;
            var currentSettingsFolder = _contentRepository.GetChildren<IContent>(settingsRoot).FirstOrDefault(x => x.Name.Equals(prevSite.Name, StringComparison.InvariantCultureIgnoreCase)) as ContentFolder;
            if (currentSettingsFolder != null)
            {
                var cloneFolder = currentSettingsFolder.CreateWritableClone();
                cloneFolder.Name = updatedSite.Name;
                _contentRepository.Save(cloneFolder);
                return;
            }

            CreateSiteFolder(e.Site);
        }

        private void PublishedContent(object sender, ContentEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            if (e.Content is SettingsBase)
            {
                var parent = _contentRepository.Get<IContent>(e.Content.ParentLink);
                var site = _siteDefinitionRepository.Get(parent.Name);

                var id = site?.Id;
                if (id == null || id == Guid.Empty)
                {
                    return;
                }
                UpdateSettings(id.Value, e.Content, false);
            }
        }

        private void SavedContent(object sender, ContentEventArgs e)
        {
            if (e == null)
            {
                return;
            }

            if (e.Content is SettingsBase)
            {
                var id = ResolveSiteId();
                if (id == Guid.Empty)
                {
                    return;
                }
                UpdateSettings(id, e.Content, true);
            }
        }

        private Guid ResolveSiteId()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
            {
                return Guid.Empty;
            }
            var site = _siteDefinitionResolver.GetByHostname(request.Host.Host, true, out var hostname);
            if (site == null)
            {
                return Guid.Empty;
            }
            return site.Id;
        }
    }
}