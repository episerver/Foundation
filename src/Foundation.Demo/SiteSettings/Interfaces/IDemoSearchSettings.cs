using Foundation.Find.Cms.Facets.Config;
using System.Collections.Generic;

namespace Foundation.Demo.SiteSettings.Interfaces
{
    public interface IDemoSearchSettings
    { 
        string SearchOption { get; set; }
        bool ShowProductSearchResults { get; set; }
        bool ShowContentSearchResults { get; set; }
        bool ShowPdfSearchResults { get; set; }
        bool IncludeImagesInContentsSearchResults { get; set; }
        int SearchCatalog { get; set; }
        IList<FacetFilterConfigurationItem> SearchFiltersConfiguration { get; set; }
    }
}
