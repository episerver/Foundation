using EPiServer;
using EPiServer.Cms.Shell.Search;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Framework.Localization;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using EPiServer.Shell.Search;
using EPiServer.Web;
using EPiServer.Web.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Cms.Settings
{
    [SearchProvider]
    public class GlobalSettingsSearchProvider : ContentSearchProviderBase<SettingsBase, ContentType>
    {
        internal const string SearchArea = "Settings/globalsettings";
        private readonly IContentLoader _contentLoader;
        private readonly LocalizationService _localizationService;
        private readonly ISettingsService _settingsService;

        public GlobalSettingsSearchProvider(
            LocalizationService localizationService,
            ISiteDefinitionResolver siteDefinitionResolver,
            IContentTypeRepository<ContentType> contentTypeRepository,
            EditUrlResolver editUrlResolver,
            ServiceAccessor<SiteDefinition> currentSiteDefinition,
            LanguageResolver languageResolver,
            UrlResolver urlResolver,
            TemplateResolver templateResolver,
            UIDescriptorRegistry uiDescriptorRegistry,
            IContentLoader contentLoader,
            ISettingsService settingsService)
            : base(
                localizationService: localizationService,
                siteDefinitionResolver: siteDefinitionResolver,
                contentTypeRepository: contentTypeRepository,
                editUrlResolver: editUrlResolver,
                currentSiteDefinition: currentSiteDefinition,
                languageResolver: languageResolver,
                urlResolver: urlResolver,
                templateResolver: templateResolver,
                uiDescriptorRegistry: uiDescriptorRegistry)
        {
            _contentLoader = contentLoader;
            _settingsService = settingsService;
            _localizationService = localizationService;
        }

        public override string Area => SearchArea;

        public override string Category => _localizationService.GetString("/episerver/cms/components/globalsettings/title");

        protected override string IconCssClass => "epi-iconSettings";

        public override IEnumerable<SearchResult> Search(Query query)
        {
            if (string.IsNullOrWhiteSpace(value: query?.SearchQuery) || query.SearchQuery.Trim().Length < 2)
            {
                return Enumerable.Empty<SearchResult>();
            }

            var searchResultList = new List<SearchResult>();
            var str = query.SearchQuery.Trim();

            var globalSettings =
                _contentLoader.GetChildren<SettingsBase>(contentLink: _settingsService.GlobalSettingsRoot);

            foreach (var setting in globalSettings)
            {
                if (setting.Name.IndexOf(value: str, comparisonType: StringComparison.OrdinalIgnoreCase) < 0)
                {
                    continue;
                }

                searchResultList.Add(CreateSearchResult(contentData: setting));

                if (searchResultList.Count == query.MaxResults)
                {
                    break;
                }
            }

            return searchResultList;
        }

        protected override string CreatePreviewText(IContentData content)
        {
            return content == null
                       ? string.Empty
                       : $"{((SettingsBase)content).Name} {_localizationService.GetString("/contentrepositories/globalsettings/customselecttitle").ToLower()}";
        }

        protected override string GetEditUrl(SettingsBase contentData, out bool onCurrentHost)
        {
            onCurrentHost = true;

            if (contentData == null)
            {
                return string.Empty;
            }

            var contentLink = contentData.ContentLink;
            var language = string.Empty;
            ILocalizable localizable = contentData;

            if (localizable != null)
            {
                language = localizable.Language.Name;
            }

            return
                $"/episerver/Foundation.Cms.Settings/settings#context=epi.cms.contentdata:///{contentLink.ID}&viewsetting=viewlanguage:///{language}";
        }
    }
}