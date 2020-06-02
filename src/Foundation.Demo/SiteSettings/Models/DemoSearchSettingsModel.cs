using Foundation.Demo.SiteSettings.Interfaces;
using Foundation.Find.Cms.Facets.Config;
using System.Collections.Generic;

namespace Foundation.Demo.SiteSettings.Models
{
    public class DemoSearchSettingsModel : IDemoSearchSettings
    {
        public string SearchOption { get; set; }
        public bool ShowProductSearchResults { get; set; }
        public bool ShowContentSearchResults { get; set; }
        public bool ShowPdfSearchResults { get; set; }
        public bool IncludeImagesInContentsSearchResults { get; set; }
        public int SearchCatalog { get; set; }
        public IList<FacetFilterConfigurationItem> SearchFiltersConfiguration { get; set; }
    }
}
