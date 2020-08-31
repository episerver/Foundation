using Foundation.Cms.Settings;
using System.Collections.Generic;

namespace Foundation.Find.Facets.Config
{
    public interface IFacetConfigFactory
    {
        List<FacetDefinition> GetDefaultFacetDefinitions();
        List<FacetFilterConfigurationItem> GetFacetFilterConfigurationItems<T>() where T : SettingsBase;
        FacetDefinition GetFacetDefinition(FacetFilterConfigurationItem facetConfiguration);
    }
}
