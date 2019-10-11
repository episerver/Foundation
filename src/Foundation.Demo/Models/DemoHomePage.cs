using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using EPiServer.Web;
using Foundation.Cms;
using Foundation.Cms.EditorDescriptors;
using Foundation.Commerce;
using Foundation.Commerce.Models.EditorDescriptors;
using Foundation.Commerce.Models.Pages;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Demo.Models
{
    [ContentType(DisplayName = "Demo Home Page",
        GUID = "452d1812-7385-42c3-8073-c1b7481e7b20",
        Description = "Used for home page of all sites",
        AvailableInEditMode = true,
        GroupName = CmsTabs.Content)]
    [ImageUrl("~/assets/icons/cms/pages/CMS-icon-page-02.png")]
    public class DemoHomePage : CommerceHomePage
    {
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            SearchCatalog = 0;
        }

        #region Site Settings

        [Display(Name = "Header Logo", GroupName = CmsTabs.SiteSettings, Order = 1)]
        [UIHint(UIHint.Image)]
        [CultureSpecific]
        public virtual ContentReference HeaderLogo { get; set; }

        [Display(Name = "Show Commerce Header Components", GroupName = CmsTabs.SiteSettings, Order = 3)]
        public virtual bool ShowCommerceHeaderComponents { get; set; }

        [Display(Name = "Show Product Ratings on all product tiles", GroupName = CmsTabs.SiteSettings, Order = 4)]
        public virtual bool ShowProductRatingsOnListings { get; set; }

        [SelectOne(SelectionFactoryType = typeof(HeaderMenuSelectionFactory))]
        [Display(Name = "Header menu style", GroupName = CmsTabs.SiteSettings, Order = 5)]
        public virtual string HeaderMenuStyle { get; set; }

        [Display(Name = "Tracking scope", GroupName = CmsTabs.SiteSettings, Order = 6)]
        public virtual string TrackingScope { get; set; }

        #endregion

        #region Search Settings

        [SelectOne(SelectionFactoryType = typeof(SearchOptionSelectionFactory))]
        [Display(Name = "Search option", GroupName = CommerceTabs.SearchSettings, Order = 50)]
        public virtual string SearchOption { get; set; }

        [Display(Name = "Show products in search results", GroupName = CommerceTabs.SearchSettings, Order = 100)]
        public virtual bool ShowProductSearchResults { get; set; }

        [Display(Name = "Show contents in search results", GroupName = CommerceTabs.SearchSettings, Order = 150)]
        public virtual bool ShowContentSearchResults { get; set; }

        [SelectOne(SelectionFactoryType = typeof(CatalogSelectionFactory))]
        [Display(Name = "Search Catalog", GroupName = CommerceTabs.SearchSettings, Order = 250,
            Description = "The catalogs that will be returned by search.")]
        public virtual int SearchCatalog { get; set; }

        #endregion
    }
}