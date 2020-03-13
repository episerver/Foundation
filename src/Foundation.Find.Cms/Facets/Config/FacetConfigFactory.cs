using EPiServer;
using EPiServer.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Find.Cms.Facets.Config
{
    public class FacetConfigFactory : IFacetConfigFactory
    {
        private readonly IContentLoader _contentLoader;

        public FacetConfigFactory(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public virtual List<FacetDefinition> GetDefaultFacetDefinitions()
        {
            return new List<FacetDefinition>();
        }

        public virtual FacetDefinition GetFacetDefinition(FacetFilterConfigurationItem facetConfiguration)
        {
            switch (Enum.Parse(typeof(FacetFieldType), facetConfiguration.FieldType))
            {
                case FacetFieldType.String:
                    return new FacetStringDefinition
                    {
                        FieldName = facetConfiguration.FieldName,
                        DisplayName = facetConfiguration.GetDisplayName(),
                        IsCommerceEnabled = true
                    };

                case FacetFieldType.ListOfString:
                    return new FacetStringListDefinition
                    {
                        FieldName = facetConfiguration.FieldName,
                        DisplayName = facetConfiguration.GetDisplayName(),
                        IsCommerceEnabled = true
                    };

                case FacetFieldType.Boolean:
                case FacetFieldType.NullableBoolean:
                    return new FacetStringListDefinition
                    {
                        FieldName = facetConfiguration.FieldName,
                        DisplayName = facetConfiguration.GetDisplayName(),
                        IsCommerceEnabled = true
                    };
            }

            return new FacetStringDefinition
            {
                FieldName = facetConfiguration.FieldName,
                DisplayName = facetConfiguration.GetDisplayName(),
                IsCommerceEnabled = true
            };
        }

        public List<FacetFilterConfigurationItem> GetFacetFilterConfigurationItems()
        {
            if (ContentReference.IsNullOrEmpty(ContentReference.StartPage))
            {
                return new List<FacetFilterConfigurationItem>();
            }

            var startPage = _contentLoader.Get<IContent>(ContentReference.StartPage);

            var facetsConfiguration = startPage as IFacetConfiguration;
            if (facetsConfiguration?.SearchFiltersConfiguration != null)
            {
                return facetsConfiguration
                    .SearchFiltersConfiguration
                    .ToList();
            }

            return new List<FacetFilterConfigurationItem>();
        }
    }
}
