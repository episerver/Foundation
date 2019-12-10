using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.Initialization;
using EPiServer.ServiceLocation;
using Foundation.Find.Cms.Facets;
using Foundation.Find.Cms.Facets.Config;
using System;
using System.Linq;

namespace Foundation.Find.Cms
{
    public static class InitializationEngineExtensions
    {
        private static Lazy<IContentEvents> _contentEvents = new Lazy<IContentEvents>(() => ServiceLocator.Current.GetInstance<IContentEvents>());
        private static Lazy<IFacetRegistry> _facetRegistry = new Lazy<IFacetRegistry>(() => ServiceLocator.Current.GetInstance<IFacetRegistry>());
        private static Lazy<IFacetConfigFactory> _facetConfigFactory = new Lazy<IFacetConfigFactory>(() => ServiceLocator.Current.GetInstance<IFacetConfigFactory>());

        public static void InitializeFoundationFindCms(this InitializationEngine context)
        {
            var configItems = _facetConfigFactory.Value.GetFacetFilterConfigurationItems();
            if (configItems.Any())
            {
                configItems.ForEach(x => _facetRegistry.Value.AddFacetDefinitions(_facetConfigFactory.Value.GetFacetDefinition(x)));
            }
            else
            {
                _facetConfigFactory.Value.GetDefaultFacetDefinitions().ForEach(x => _facetRegistry.Value.AddFacetDefinitions(x));
            }

            _contentEvents.Value.PublishedContent += OnPublishedContent;

        }

        static void OnPublishedContent(object sender, ContentEventArgs contentEventArgs)
        {
            if (contentEventArgs.Content is IFacetConfiguration facetConfiguration)
            {
                _facetRegistry.Value.Clear();
                facetConfiguration.SearchFiltersConfiguration
                    .ToList()
                    .ForEach(x => _facetRegistry.Value.AddFacetDefinitions(_facetConfigFactory.Value.GetFacetDefinition(x)));
            }
        }
    }
}
