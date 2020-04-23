using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.ServiceLocation;
using Foundation.Cms;
using Foundation.Cms.SiteSettings;
using Foundation.Cms.ViewModels;
using Foundation.Commerce;
using Foundation.Commerce.Models.Pages;
using Foundation.Demo.ViewModels;
using Foundation.Find.Cms.Facets.Config;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
        [CultureSpecific]
        [Display(Name = "Settings page", GroupName = CommerceTabNames.SiteStructure, Order = 100)]
        [AllowedTypes(new[] { typeof(DemoSettingsPage) })]
        public override ContentReference SettingsPage { get; set; }
        
        // Get settings
        public override CmsSiteSettings SiteSettings
        {
            get
            {
                var _siteSettingsProvider = ServiceLocator.Current.GetInstance<ISiteSettingsProvider>();
                return _siteSettingsProvider.GetSiteSettings<DemoSiteSettings>(this);
            }
        }

        public virtual ContentReference SiteLogo => (SiteSettings as DemoSiteSettings).SiteLogo;
        public virtual string HeaderMenuStyle => (SiteSettings as DemoSiteSettings).HeaderMenuStyle;
        public virtual bool LargeHeaderMenu => (SiteSettings as DemoSiteSettings).LargeHeaderMenu;
        public virtual bool ShowCommerceHeaderComponents => (SiteSettings as DemoSiteSettings).ShowCommerceHeaderComponents;
        public virtual string SearchOption => (SiteSettings as DemoSiteSettings).SearchOption;
        public virtual bool ShowProductSearchResults => (SiteSettings as DemoSiteSettings).ShowProductSearchResults;
        public virtual bool ShowContentSearchResults => (SiteSettings as DemoSiteSettings).ShowContentSearchResults;
        public virtual bool IncludeImagesInContentsSearchResults => (SiteSettings as DemoSiteSettings).IncludeImagesInContentsSearchResults;
        public virtual int SearchCatalog => (SiteSettings as DemoSiteSettings).SearchCatalog;
        public virtual IList<FacetFilterConfigurationItem> SearchFiltersConfiguration => (SiteSettings as DemoSiteSettings).SearchFiltersConfiguration;
        public virtual bool StickyTopHeader => (SiteSettings as DemoSiteSettings).StickyTopHeader;
    }
}