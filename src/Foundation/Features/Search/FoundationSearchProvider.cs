// EPiServer.Find removed: FoundationSearchProvider stubbed to return empty results.
// Phase 4 will restore Find/Graph-based catalog search provider.
using EPiServer.Applications;
using EPiServer.Cms.Shell.Search;
using EPiServer.Framework.Modules;
using EPiServer.Logging;
using EPiServer.Shell;
using EPiServer.Shell.Search;
using Foundation.Features.CatalogContent.Product;
using Mediachase.Commerce.Core;
using Mediachase.Search;
using System.ComponentModel;

namespace Foundation.Features.Search
{
    [SearchProvider]
    [Browsable(false)]
    public class FoundationSearchProvider : ContentSearchProviderBase<EntryContentBase, ContentType>
    {
        [NonSerialized]
        private readonly ILogger _log = LogManager.GetLogger(typeof(FoundationSearchProvider));

        private readonly LocalizationService _localizationService;
        private readonly IContentLanguageAccessor _contentLanguageAccessor;
        private readonly Mediachase.Commerce.Catalog.ReferenceConverter _referenceConverter;
        private readonly IContentLoader _contentLoader;
        private readonly ServiceAccessor<SiteContext> _siteContextAcessor;
        private readonly ServiceAccessor<SearchManager> _searchManagerAccessor;
        internal static readonly string SearchArea = "Commerce/Catalog";

        public FoundationSearchProvider(
            LocalizationService localizationService,
            IApplicationResolver applicationResolver,
            IContentTypeRepository contentTypeRepository, // CMS 13: IContentTypeRepository is no longer generic.
            EditUrlResolver editUrlResolver,
            IContentLanguageAccessor contentLanguageAccessor,
            UrlResolver urlResolver,
            UIDescriptorRegistry uiDescriptorRegistry,
            Mediachase.Commerce.Catalog.ReferenceConverter referenceConverter,
            ServiceAccessor<SearchManager> searchManagerAccessor,
            IContentLoader contentLoader,
            IModuleResourceResolver moduleResourceResolver,
            ServiceAccessor<SiteContext> siteContextAccessor) :
                // CMS 13: ContentSearchProviderBase constructor changed. siteDefinitionResolver, currentSiteDefinition, templateResolver removed.
                base(localizationService,
                    applicationResolver,
                    contentTypeRepository,
                    editUrlResolver,
                    contentLanguageAccessor,
                    urlResolver,
                    uiDescriptorRegistry)
        {
            _contentLanguageAccessor = contentLanguageAccessor;
            _localizationService = localizationService;
            _referenceConverter = referenceConverter;
            _searchManagerAccessor = searchManagerAccessor;
            _contentLoader = contentLoader;
            _siteContextAcessor = siteContextAccessor;
            EditPath = (contentData, contentLink, languageName) =>
            {
                var catalogPath = moduleResourceResolver.ResolvePath("Commerce", "Catalog");
                return $"{catalogPath}#context=epi.cms.contentdata:///{contentLink}";
            };
        }

        public override string Area => SearchArea;

        public override string Category => _localizationService.GetString("/Commerce/Edit/Provider/SearchProductCatalog/Category");

        protected override string IconCssClass => "epi-resourceIcon epi-resourceIcon-page";

        // EPiServer.Find removed: returns empty results. Phase 4 will restore Graph-based catalog search.
        public override IEnumerable<SearchResult> Search(Query query)
        {
            return Enumerable.Empty<SearchResult>();
        }
    }
}
