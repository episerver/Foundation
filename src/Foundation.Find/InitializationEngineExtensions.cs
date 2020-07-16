using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Foundation.Find.Facets;
using Foundation.Find.Facets.Config;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Find
{
    public static class InitializationEngineExtensions
    {
        private static Lazy<IContentEvents> _contentEvents = new Lazy<IContentEvents>(() => ServiceLocator.Current.GetInstance<IContentEvents>());
        private static Lazy<IFacetRegistry> _facetRegistry = new Lazy<IFacetRegistry>(() => ServiceLocator.Current.GetInstance<IFacetRegistry>());
        private static Lazy<IFacetConfigFactory> _facetConfigFactory = new Lazy<IFacetConfigFactory>(() => ServiceLocator.Current.GetInstance<IFacetConfigFactory>());

        public static void InitializeFoundationFindCms(this InitializationEngine context)
        {
            InitializeFacets(_facetConfigFactory.Value.GetFacetFilterConfigurationItems());

            _contentEvents.Value.PublishedContent += OnPublishedContent;
        }

        static void OnPublishedContent(object sender, ContentEventArgs contentEventArgs)
        {
            if (contentEventArgs.Content is IFacetConfiguration facetConfiguration)
            {
                InitializeFacets(facetConfiguration.SearchFiltersConfiguration);
            }
        }

        private static void InitializeFacets(IList<FacetFilterConfigurationItem> configItems)
        {
            _facetRegistry.Value.Clear();

            if (configItems != null && configItems.Any())
            {
                configItems
                    .ToList()
                    .ForEach(x => _facetRegistry.Value.AddFacetDefinitions(_facetConfigFactory.Value.GetFacetDefinition(x)));
            }
            else
            {
                _facetConfigFactory.Value.GetDefaultFacetDefinitions()
                    .ForEach(x => _facetRegistry.Value.AddFacetDefinitions(x));
            }
        }
    }
}
