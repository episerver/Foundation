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
using Foundation.Find.Cms.Facets;
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
    public class DemoHomePage : CommerceHomePage, IFacetConfiguration
    {
        #region Header

        [CultureSpecific]
        [UIHint(UIHint.Image)]
        [Display(Name = "Site logo", GroupName = CmsTabNames.Header, Order = 10)]
        public virtual ContentReference SiteLogo { get; set; }

        [SelectOne(SelectionFactoryType = typeof(HeaderMenuSelectionFactory))]
        [Display(Name = "Menu style", GroupName = CmsTabNames.Header, Order = 30)]
        public virtual string HeaderMenuStyle { get; set; }

        [Display(Name = "Large header menu", GroupName = CmsTabNames.Header, Order = 35)]
        public virtual bool LargeHeaderMenu { get; set; }

        [Display(Name = "Show commerce header components", GroupName = CmsTabNames.Header, Order = 40)]
        public virtual bool ShowCommerceHeaderComponents { get; set; }

        [Display(Name = "Sticky header", GroupName = CmsTabNames.Header, Order = 50)]
        public virtual bool StickyTopHeader { get; set; }

        #endregion

        #region Search Settings

        [SelectOne(SelectionFactoryType = typeof(SearchOptionSelectionFactory))]
        [Display(Name = "Search option", GroupName = CommerceTabNames.SearchSettings, Order = 50)]
        public virtual string SearchOption { get; set; }

        [Display(Name = "Show products in search results", GroupName = CommerceTabNames.SearchSettings, Order = 100)]
        public virtual bool ShowProductSearchResults { get; set; }

        [Display(Name = "Show contents in search results", GroupName = CommerceTabNames.SearchSettings, Order = 150)]
        public virtual bool ShowContentSearchResults { get; set; }

        [Display(Name = "Show PDFs in search results", GroupName = CommerceTabNames.SearchSettings, Order = 175)]
        public virtual bool ShowPdfSearchResults { get; set; }

        [Display(Name = "Include images in contents search results", GroupName = CommerceTabNames.SearchSettings, Order = 200)]
        public virtual bool IncludeImagesInContentsSearchResults { get; set; }

        [SelectOne(SelectionFactoryType = typeof(CatalogSelectionFactory))]
        [Display(Name = "Search catalog", GroupName = CommerceTabNames.SearchSettings, Order = 250,
            Description = "The catalogs that will be returned by search.")]
        public virtual int SearchCatalog { get; set; }

        [Display(
          Name = "Search Filters Configuration",
          Description = "Manage filters to be displayed on Search",
          GroupName = CommerceTabNames.SearchSettings,
          Order = 300)]
        [EditorDescriptor(EditorDescriptorType = typeof(IgnoreCollectionEditorDescriptor<FacetFilterConfigurationItem>))]
        public virtual IList<FacetFilterConfigurationItem> SearchFiltersConfiguration { get; set; }

        #endregion

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            LargeHeaderMenu = false;
            SearchCatalog = 0;
        }
    }
}