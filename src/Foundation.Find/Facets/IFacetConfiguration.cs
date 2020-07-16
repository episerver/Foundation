using Foundation.Find.Facets.Config;
using System.Collections.Generic;

namespace Foundation.Find.Facets
{
    public interface IFacetConfiguration
    {
        IList<FacetFilterConfigurationItem> SearchFiltersConfiguration { get; set; }
    }
}
