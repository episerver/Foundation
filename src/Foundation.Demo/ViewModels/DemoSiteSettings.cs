using EPiServer.Core;
using Foundation.Commerce.ViewModels;
using Foundation.Find.Cms.Facets.Config;
using System.Collections.Generic;

namespace Foundation.Demo.ViewModels
{
    public class DemoSiteSettings : CommerceSiteSettings
    {
        public virtual ContentReference SiteLogo { get; set; }
        public virtual string HeaderMenuStyle { get; set; }
        public virtual bool LargeHeaderMenu { get; set; }
        public virtual bool ShowCommerceHeaderComponents { get; set; }
        public virtual string SearchOption { get; set; }
        public virtual bool ShowProductSearchResults { get; set; }
        public virtual bool ShowContentSearchResults { get; set; }
        public virtual bool IncludeImagesInContentsSearchResults { get; set; }
        public virtual int SearchCatalog { get; set; }
        public virtual IList<FacetFilterConfigurationItem> SearchFiltersConfiguration { get; set; }
        public virtual bool StickyTopHeader { get; set; }
    }
}
