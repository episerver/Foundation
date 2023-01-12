using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Find;
using EPiServer.Find.Cms;
using EPiServer.Find.Commerce;
using EPiServer.Find.Commerce.Services;
using System.Globalization;

namespace Foundation.Features.Search
{
    public class FoundationCatalogContentEventListener : CatalogContentEventListener
    {
        private readonly IContentRepository _contentRepository;
        private readonly IRelationRepository _relationRepository;

        public FoundationCatalogContentEventListener(ReferenceConverter referenceConverter,
           IContentRepository contentRepository,
           IClient client,
           CatalogEventIndexer indexer,
           CatalogContentClientConventions catalogContentClientConventions,
           PriceIndexing priceIndexing,
           IRelationRepository relationRepository,
           EventedIndexingSettings eventedIndexingSettings) :
            base(referenceConverter, contentRepository, client, indexer, catalogContentClientConventions, priceIndexing, eventedIndexingSettings)
        {
            _contentRepository = contentRepository;
            _relationRepository = relationRepository;
        }

        protected override void IndexContentsIfNeeded(IEnumerable<ContentReference> contentLinks, IDictionary<Type, bool> cachedReindexContentOnEventForType,
           Func<bool> isReindexingContentOnUpdates)
        {
            // Update parent contents
            var contents = _contentRepository.GetItems(contentLinks, CultureInfo.InvariantCulture).ToList();
            var parentContentLinks = new List<ContentReference>();
            foreach (var parents in contents.OfType<VariationContent>().Select(content => _contentRepository.GetItems(content.GetParentProducts(_relationRepository), CultureInfo.InvariantCulture)
                .Select(c => c.ContentLink).ToList()))
            {
                parentContentLinks.AddRange(parents);
            }
            IndexContentsIfNeeded(parentContentLinks, GetIndexContentAction());
        }
    }
}