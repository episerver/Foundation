using Foundation.Find.Cms.Facets.Config;
using System.Collections.Generic;

namespace Foundation.Find.Cms.Facets
{
    public interface IFacetConfiguration
    {
        IList<FacetFilterConfigurationItem> SearchFiltersConfiguration { get; set; }
    }
}
