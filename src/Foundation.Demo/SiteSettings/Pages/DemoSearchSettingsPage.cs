using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms.EditorDescriptors;
using Foundation.Cms.SiteSettings.Pages;
using Foundation.Commerce;
using Foundation.Commerce.Models.EditorDescriptors;
using Foundation.Demo.SiteSettings.Interfaces;
using Foundation.Find.Cms.Facets;
using Foundation.Find.Cms.Facets.Config;

namespace Foundation.Demo.SiteSettings.Pages
{
    [ContentType(DisplayName = "Demo Search Settings Page", 
        GUID = "14f5b59c-919c-432e-9bcf-acaac4a2b100", 
        Description = "")]
    public class DemoSearchSettingsPage : SettingsBasePage, IFacetConfiguration, IDemoSearchSettings
    {

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
            SearchCatalog = 0;
        }
    }
}