using Foundation.Infrastructure.Find.Facets.Config;
using System.Collections.Generic;

namespace Foundation.Infrastructure.Find.Facets
{
    public interface IFacetConfiguration
    {
        IList<FacetFilterConfigurationItem> SearchFiltersConfiguration { get; set; }
    }
}
