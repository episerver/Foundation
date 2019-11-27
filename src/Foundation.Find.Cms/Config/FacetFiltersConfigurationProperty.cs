using EPiServer.PlugIn;

namespace Foundation.Find.Cms.Config
{
    [PropertyDefinitionTypePlugIn]
    public class FacetFiltersConfigurationProperty : PropertyListBase<FacetFilterProductConfigurationItem>
    {
    }

    [PropertyDefinitionTypePlugIn]
    public class FacetContentFiltersConfigurationProperty : PropertyListBase<FacetFilterContentConfigurationItem>
    {
    }
}
