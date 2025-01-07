using System.Collections.Concurrent;
using System.Globalization;
using EPiServer.DataAccess;
using EPiServer.Events;
using EPiServer.Events.Clients;
using EPiServer.Framework.TypeScanner;
using EPiServer.Security;
using Microsoft.Extensions.Logging;

namespace Foundation.Infrastructure.Cms.Settings;
public class SettingsService : ISettingsService, IDisposable
{
    //Generate unique id for your event and the raiser
    private readonly Guid _raiserId;
    private static Guid EventId => new("b29b8aef-2a17-4432-ad16-8cd6cc6953e3");

    private readonly IContentRepository _contentRepository;
    private readonly ContentRootService _contentRootService;
    private readonly IContentTypeRepository _contentTypeRepository;
    private readonly ILogger _log;
    private readonly ITypeScannerLookup _typeScannerLookup;
    private readonly IContentEvents _contentEvents;
    private readonly ISiteDefinitionEvents _siteDefinitionEvents;
    private readonly ISiteDefinitionRepository _siteDefinitionRepository;
    private readonly ISiteDefinitionResolver _siteDefinitionResolver;
    private readonly IHttpContextAccessor _httpContext;
    private readonly IEventRegistry _eventRegistry;
    private readonly IContentLanguageAccessor _contentLanguageAccessor;

    public SettingsService(
        IContentRepository contentRepository,
        ContentRootService contentRootService,
        ITypeScannerLookup typeScannerLookup,
        IContentTypeRepository contentTypeRepository,
        IContentEvents contentEvents,
        ISiteDefinitionEvents siteDefinitionEvents,
        ISiteDefinitionRepository siteDefinitionRepository,
        ISiteDefinitionResolver siteDefinitionResolver,
        IHttpContextAccessor httpContext,
        IEventRegistry eventRegistry,
        IContentLanguageAccessor contentLanguageAccessor,
        ILogger<SettingsService> logger)
    {
        _contentRepository = contentRepository;
        _contentRootService = contentRootService;
        _typeScannerLookup = typeScannerLookup;
        _contentTypeRepository = contentTypeRepository;
        _contentEvents = contentEvents;
        _siteDefinitionEvents = siteDefinitionEvents;
        _siteDefinitionRepository = siteDefinitionRepository;
        _siteDefinitionResolver = siteDefinitionResolver;
        _httpContext = httpContext;
        _eventRegistry = eventRegistry;
        _raiserId = Guid.NewGuid();
        _contentLanguageAccessor = contentLanguageAccessor;
        _log = logger;
    }

    public ConcurrentDictionary<Guid, Dictionary<Type, Guid>> SiteSettings { get; } = new ConcurrentDictionary<Guid, Dictionary<Type, Guid>>();

    public ContentReference GlobalSettingsRoot { get; set; }

    public List<T> GetAllSiteSettings<T>() where T : SettingsBase
    {
        var sites = _siteDefinitionRepository.List();
        var siteSettings = new List<T>();

        foreach (var site in sites)
        {
            var settings = GetSiteSettings<T>(site.Id);
            if (settings != null)
            {
                siteSettings.Add(settings);
            }
        }

        return siteSettings;
    }

    public T GetSiteSettings<T>(Guid? siteId = null, string language = null) where T : SettingsBase
    {
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
            if (SiteSettings.TryGetValue(siteId.Value, out var siteSettings) &&
                siteSettings.TryGetValue(typeof(T), out var settingId))
            {
                return _contentRepository.Get<T>(settingId, language == null ? _contentLanguageAccessor.Language : CultureInfo.GetCultureInfo(language));
            }
        }
        catch (KeyNotFoundException keyNotFoundException)
        {
            _log.LogError(keyNotFoundException, $"[Settings] {keyNotFoundException.Message}");
        }
        catch (ArgumentNullException argumentNullException)
        {
            _log.LogError(argumentNullException, $"[Settings] {argumentNullException.Message}");
        }

        return default;
    }

    public void InitializeSettings()
    {
        try
        {
            RegisterContentRoots();
        }
        catch (NotSupportedException notSupportedException)
        {
            _log.LogError(notSupportedException, $"[Settings] {notSupportedException.Message}");
            throw;
        }
        catch (InvalidOperationException ex)
        {
            _log.LogError(ex, ex.Message);
        }
        _contentEvents.PublishedContent += PublishedContent;
        _siteDefinitionEvents.SiteCreated += SiteCreated;
        _siteDefinitionEvents.SiteUpdated += SiteUpdated;
        _siteDefinitionEvents.SiteDeleted += SiteDeleted;

        var settingsEvent = _eventRegistry.Get(EventId);
        settingsEvent.Raised += SettingsEvent_Raised;
    }

    public void UpdateSettings(Guid siteId, IContent content)
    {
        var contentType = content.GetOriginalType();
        try
        {
            if (!SiteSettings.ContainsKey(siteId))
            {
                SiteSettings[siteId] = new Dictionary<Type, Guid>();
            }

            SiteSettings[siteId][contentType] = content.ContentGuid;
        }
        catch (KeyNotFoundException keyNotFoundException)
        {
            _log.LogError(keyNotFoundException, $"[Settings] {keyNotFoundException.Message}");
        }
        catch (ArgumentNullException argumentNullException)
        {
            _log.LogError(argumentNullException, $"[Settings] {argumentNullException.Message}");
        }
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
            if (folder == null)
            {
                CreateSiteFolder(site);
                return;
            }

            var settingsModelTypes = _typeScannerLookup.AllTypes
                    .Where(t => t.GetCustomAttributes(typeof(SettingsContentTypeAttribute), false).Length > 0);

            foreach (var settingsType in settingsModelTypes)
            {
                if (settingsType.GetCustomAttributes(typeof(SettingsContentTypeAttribute), false)
                    .FirstOrDefault() is not SettingsContentTypeAttribute attribute)
                {
                    continue;
                }

                var siteSetting = _contentRepository.GetChildren<SettingsBase>(folder.ContentLink, _contentLanguageAccessor.Language)
                    .FirstOrDefault(x => x.Name.Equals(attribute.SettingsName));

                if (siteSetting != null)
                {
                    UpdateSettings(site.Id, siteSetting);
                }
                else
                {
                    var contentType = _contentTypeRepository.Load(settingsType);
                    var newSettings = _contentRepository.GetDefault<IContent>(folder.ContentLink, contentType.ID);
                    newSettings.Name = attribute.SettingsName;

                    try
                    {
                        _ = _contentRepository.Save(newSettings, SaveAction.Publish, AccessLevel.NoAccess);
                        UpdateSettings(site.Id, newSettings);
                    }
                    catch (Exception e)
                    {
                        _log.LogError(e.Message);
                    }
                }

            }
        }
    }

    public void RegisterContentRoots()
    {
        var registeredRoots = _contentRepository.GetItems(_contentRootService.List(), new LoaderOptions());
        var settingsRootRegistered = registeredRoots.Any(x => x.ContentGuid == SettingsFolder.SettingsRootGuid && x.Name.Equals(SettingsFolder.SettingsRootName));

        if (!settingsRootRegistered)
        {
            _contentRootService.Register<SettingsFolder>(SettingsFolder.SettingsRootName, SettingsFolder.SettingsRootGuid, ContentReference.RootPage);
        }

        UpdateSettings();
    }

    public void Dispose()
    {
        if (_contentEvents != null)
        {
            _contentEvents.PublishedContent -= PublishedContent;
        }

        if (_siteDefinitionEvents != null)
        {
            _siteDefinitionEvents.SiteCreated -= SiteCreated;
            _siteDefinitionEvents.SiteUpdated -= SiteUpdated;
            _siteDefinitionEvents.SiteDeleted -= SiteDeleted;
        }

        if (_eventRegistry != null)
        {
            var settingsEvent = _eventRegistry.Get(EventId);
            settingsEvent.Raised -= SettingsEvent_Raised;
        }

        GC.SuppressFinalize(this);
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
            if (settingsType.GetCustomAttributes(typeof(SettingsContentTypeAttribute), false)
                .FirstOrDefault() is not SettingsContentTypeAttribute attribute)
            {
                continue;
            }

            var contentType = _contentTypeRepository.Load(settingsType);
            var newSettings = _contentRepository.GetDefault<IContent>(reference, contentType.ID);
            newSettings.Name = attribute.SettingsName;

            try
            {
                _contentRepository.Save(newSettings, SaveAction.Publish, AccessLevel.NoAccess);
                UpdateSettings(siteDefinition.Id, newSettings);
            }
            catch (Exception e)
            {
                _log.LogError(e.Message);
            }
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
        if (e is SiteDefinitionUpdatedEventArgs updatedArgs)
        {
            var prevSite = updatedArgs.PreviousSite;
            var updatedSite = updatedArgs.Site;
            var settingsRoot = GlobalSettingsRoot;
            if (_contentRepository.GetChildren<IContent>(settingsRoot)
                .FirstOrDefault(x => x.Name.Equals(prevSite.Name, StringComparison.InvariantCultureIgnoreCase)) is ContentFolder currentSettingsFolder)
            {
                var cloneFolder = currentSettingsFolder.CreateWritableClone();
                cloneFolder.Name = updatedSite.Name;
                _contentRepository.Save(cloneFolder);
                return;
            }
        }

        CreateSiteFolder(e.Site);
    }

    private void PublishedContent(object sender, ContentEventArgs e)
    {
        if (e?.Content is not SettingsBase)
        {
            return;
        }

        var parent = _contentRepository.Get<IContent>(e.Content.ParentLink);
        var site = _siteDefinitionRepository.Get(parent.Name);

        var id = site?.Id;
        if (id == null || id == Guid.Empty)
        {
            return;
        }
        UpdateSettings(id.Value, e.Content);
        RaiseEvent(new SettingEventData
        {
            SiteId = id.ToString(),
            ContentId = e.Content.ContentGuid.ToString()
        });
    }

    private Guid ResolveSiteId()
    {
        var request = _httpContext.HttpContext?.Request;
        if (request == null)
        {
            return Guid.Empty;
        }

        var site = _siteDefinitionResolver.GetByHostname(request.Host.Value, false, out _);
        if (site != null)
        {
            return site.Id;
        }

        site = _siteDefinitionRepository.List()
            .FirstOrDefault(x => x.Hosts.Any(x => x.Url.Host.Equals(request.Host.Value, StringComparison.OrdinalIgnoreCase)));

        if (site != null)
        {
            return site.Id;
        }

        return Guid.Empty;
    }

    private void SettingsEvent_Raised(object sender, EventNotificationEventArgs e)
    {
        // don't process events locally raised
        if (e.RaiserId != _raiserId && e.Param is SettingEventData settingUpdate && settingUpdate != null)
        {
            if (Guid.TryParse(settingUpdate.ContentId, out var contentId))
            {
                var content = _contentRepository.Get<IContent>(contentId);
                if (content != null && settingUpdate.SiteId != null)
                {
                    UpdateSettings(Guid.Parse(settingUpdate.SiteId), content);
                }
            }
        }
    }

    private void RaiseEvent(SettingEventData message) => _eventRegistry.Get(EventId).Raise(_raiserId, message);

}
