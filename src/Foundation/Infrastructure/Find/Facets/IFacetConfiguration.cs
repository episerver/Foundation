using Foundation.Infrastructure.Find.Facets.Config;

namespace Foundation.Infrastructure.Find.Facets
{
    public interface IFacetConfiguration
    {
        IList<FacetFilterConfigurationItem> SearchFiltersConfiguration { get; set; }
    }
}
