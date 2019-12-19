using System.Collections.Generic;

namespace Foundation.Find.Cms.Facets.Config
{
    public interface IFacetConfigFactory
    {
        List<FacetDefinition> GetDefaultFacetDefinitions();
        List<FacetFilterConfigurationItem> GetFacetFilterConfigurationItems();
        FacetDefinition GetFacetDefinition(FacetFilterConfigurationItem facetConfiguration);
    }
}
