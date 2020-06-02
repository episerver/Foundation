using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.ServiceLocation;
using Foundation.Cms;
using Foundation.Cms.SiteSettings.Interfaces;
using Foundation.Commerce.Models.Pages;
using Foundation.Demo.SiteSettings.Models;
using Foundation.Find.Cms.Facets.Config;
using System.Collections.Generic;

namespace Foundation.Demo.Models
{
    [ContentType(DisplayName = "Demo Home Page",
        GUID = "452d1812-7385-42c3-8073-c1b7481e7b20",
        Description = "Used for home page of all sites",
        AvailableInEditMode = true,
        GroupName = CmsGroupNames.Content)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-02.png")]
    public class DemoHomePage : CommerceHomePage
    {
        #region Settings properties
        private DemoSettingsModel DemoSiteSettings
        {
            get
            {
                var siteSettingsProvider = ServiceLocator.Current.GetInstance<ISiteSettingsProvider>();
                var settings = siteSettingsProvider.GetSiteSettings<DemoSettingsModel>(this.SettingNode);
                return settings;
            }
        }

        public ContentReference SiteLogo => DemoSiteSettings.DemoLayoutSettings.SiteLogo;
        public string HeaderMenuStyle => DemoSiteSettings.DemoLayoutSettings.HeaderMenuStyle;
        public bool LargeHeaderMenu => DemoSiteSettings.DemoLayoutSettings.LargeHeaderMenu;
        public bool ShowCommerceHeaderComponents => DemoSiteSettings.DemoLayoutSettings.ShowCommerceHeaderComponents;
        public bool StickyTopHeader => DemoSiteSettings.DemoLayoutSettings.StickyTopHeader;

        public string SearchOption => DemoSiteSettings.SearchSettings.SearchOption;
        public bool ShowProductSearchResults => DemoSiteSettings.SearchSettings.ShowProductSearchResults;
        public bool ShowContentSearchResults => DemoSiteSettings.SearchSettings.ShowContentSearchResults;
        public bool ShowPdfSearchResults => DemoSiteSettings.SearchSettings.ShowPdfSearchResults;
        public bool IncludeImagesInContentsSearchResults => DemoSiteSettings.SearchSettings.IncludeImagesInContentsSearchResults;
        public int SearchCatalog => DemoSiteSettings.SearchSettings.SearchCatalog;
        public IList<FacetFilterConfigurationItem> SearchFiltersConfiguration => DemoSiteSettings.SearchSettings.SearchFiltersConfiguration;
        #endregion
    }
}