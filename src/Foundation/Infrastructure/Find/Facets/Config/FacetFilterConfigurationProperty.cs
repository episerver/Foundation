using EPiServer.Core;
using EPiServer.PlugIn;
using Newtonsoft.Json;

namespace Foundation.Infrastructure.Find.Facets.Config
{
    // CMS 13: extends PropertyList<T> directly (bypassing PropertyListBase<T> open-generic chain)
    // to ensure PropertyDefinitionTypeResolver can map IList<FacetFilterConfigurationItem> correctly.
    [PropertyDefinitionTypePlugIn]
    public class FacetFilterConfigurationProperty : PropertyList<FacetFilterConfigurationItem>
    {
        protected override FacetFilterConfigurationItem ParseItem(string value)
        {
            return JsonConvert.DeserializeObject<FacetFilterConfigurationItem>(value);
        }
    }
}
