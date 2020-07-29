using EPiServer;
using EPiServer.Cms.Shell.Search;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.DataAbstraction;
using EPiServer.Find;
using EPiServer.Find.Api.Querying;
using EPiServer.Find.Cms;
using EPiServer.Find.Commerce;
using EPiServer.Framework.Localization;
using EPiServer.Framework.Modules;
using EPiServer.Globalization;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using EPiServer.Shell;
using EPiServer.Shell.Search;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Foundation.Commerce.Extensions;
using Foundation.Features.CatalogContent.Product;
using Foundation.Find;
using Mediachase.Commerce.Core;
using Mediachase.Search;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Foundation.Features.Search
{
    [SearchProvider]
    [Browsable(false)]
    public class FoundationSearchProvider : ContentSearchProviderBase<EntryContentBase, ContentType>
    {
        private const int StartRowIndex = 0;
        [NonSerialized]
        private readonly ILogger _log = LogManager.GetLogger(typeof(FoundationSearchProvider));

        private readonly LocalizationService _localizationService;
        private readonly LanguageResolver _languageResolver;
        private readonly Mediachase.Commerce.Catalog.ReferenceConverter _referenceConverter;
        private readonly IContentLoader _contentLoader;
        private readonly ServiceAccessor<SiteContext> _siteContextAcessor;
        private readonly ServiceAccessor<SearchManager> _searchManagerAccessor;
        private readonly IClient _client;
        internal static readonly string SearchArea = "Commerce/Catalog";

        public FoundationSearchProvider(
            LocalizationService localizationService,
            ISiteDefinitionResolver siteDefinitionResolver,
            IContentTypeRepository<ContentType> contentTypeRepository,
            EditUrlResolver editUrlResolver,
            ServiceAccessor<SiteDefinition> currentSiteDefinition,
            LanguageResolver languageResolver,
            UrlResolver urlResolver,
            TemplateResolver templateResolver,
            UIDescriptorRegistry uiDescriptorRegistry,
            Mediachase.Commerce.Catalog.ReferenceConverter referenceConverter,
            ServiceAccessor<SearchManager> searchManagerAccessor,
            IContentLoader contentLoader,
            IModuleResourceResolver moduleResourceResolver,
            ServiceAccessor<SiteContext> siteContextAccessor,
            IClient client) :
                base(localizationService,
                    siteDefinitionResolver,
                    contentTypeRepository,
                    editUrlResolver,
                    currentSiteDefinition,
                    languageResolver,
                    urlResolver,
                    templateResolver,
                    uiDescriptorRegistry)
        {
            _languageResolver = languageResolver;
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
            _client = client;
        }

        /// <summary>
        /// The search area where this provider will search.
        /// </summary>
        /// <see cref="SearchArea"/>
        public override string Area => SearchArea;

        /// <summary>
        /// Category display
        /// </summary>
        public override string Category => _localizationService.GetString("/Commerce/Edit/Provider/SearchProductCatalog/Category");

        /// <summary>
        /// Gets the icon CSS class.
        /// </summary>
        protected override string IconCssClass => "epi-resourceIcon epi-resourceIcon-page";

        /// <summary>
        /// Search in ProductCatalog and return list of result
        /// </summary>
        /// <param name="query">input query text and max number of result display</param>
        /// <returns>IEnumerable<SearchResult/> display total search result</returns>
        public override IEnumerable<SearchResult> Search(Query query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query), "query cannot be null");
            }

            if (string.IsNullOrWhiteSpace(query.SearchQuery))
            {
                return Enumerable.Empty<SearchResult>();
            }

            try
            {
                return SearchEntries(query.SearchQuery, query.MaxResults);
            }
            catch (Exception ex)
            {
                _log.Error("Error when processing search product catalog query", ex);
                return Enumerable.Empty<SearchResult>();
            }
        }

        protected IEnumerable<SearchResult> SearchEntries(string keyword, int pageSize)
        {
            return CreateSearchResults(_client.Search<EntryContentBase>()
                .Take(pageSize)
                .OrFilter(_ => _.Code.PrefixCaseInsensitive(keyword) | _.Name.PrefixCaseInsensitive(keyword))
                .OrFilter(_ => _.MatchTypeHierarchy(typeof(GenericProduct)) & (((GenericProduct)_).VariationContents().PrefixCaseInsensitive(x => x.Code, keyword)))
                .OrFilter(_ => _.MatchTypeHierarchy(typeof(GenericProduct)) & (((GenericProduct)_).VariationContents().PrefixCaseInsensitive(x => x.DisplayName, keyword)))
                .GetContentResult(), keyword);
        }

        private IEnumerable<SearchResult> CreateSearchResults(IEnumerable<EntryContentBase> documents, string keyword)
        {
            var culture = _languageResolver.GetPreferredCulture();
            var references = documents.Select(_ => _.ContentLink)
                .ToList();

            var childReferences = documents.OfType<GenericProduct>()
                .SelectMany(x => x.Variations())
                .Select(x => x)
                .ToList();

            var entries = _contentLoader.GetItems(childReferences, culture)
                .OfType<EntryContentBase>()
                .Where(x => x.Name.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0 ||
                    x.Code.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0);

            foreach (var entry in documents)
            {
                yield return CreateMySearchResult(entry);
            }

            foreach (var entry in entries)
            {
                yield return CreateMySearchResult(entry);
            }
        }

        private SearchResult CreateMySearchResult(EntryContentBase entry)
        {
            var result = base.CreateSearchResult(entry);
            result.Metadata.Add("parentType", _referenceConverter.GetContentType(entry.ParentLink).ToString());
            result.Metadata.Add("code", entry.Code);
            return result;
        }
    }

    public static class SearchExtensions
    {
        public static ITypeSearch<TSource> OrFilter<TSource, TListItem>(this ITypeSearch<TSource> search, Expression<Func<TSource, IEnumerable<TListItem>>> nestedExpression,
            Expression<Func<TListItem, Filter>> filterExpression)
        {
            var filter = new FilterExpressionParser(search.Client.Conventions)
                .GetFilter(new NestedFilterExpression<TSource, TListItem>(nestedExpression, filterExpression, search.Client.Conventions).Expression);

            return search.OrFilter(filter);
        }
    }
}