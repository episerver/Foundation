using Foundation.Infrastructure.Cms.Settings;
using Foundation.Infrastructure.Commerce.Models.EditorDescriptors;
using Foundation.Infrastructure.Find.Facets;
using Foundation.Infrastructure.Find.Facets.Config;

namespace Foundation.Features.Settings
{
    [SettingsContentType(DisplayName = "Search & Catalog Settings",
        GUID = "d4171337-70a4-476a-aa3c-0d976ac185e8",
        SettingsName = "Search Settings")]
    public class SearchSettings : SettingsBase, IFacetConfiguration
    {
        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(SearchOptionSelectionFactory))]
        [Display(Name = "Search option", GroupName = TabNames.SearchSettings, Order = 50)]
        public virtual string SearchOption { get; set; }

        [CultureSpecific]
        [Display(Name = "Show products in search results", GroupName = TabNames.SearchSettings, Order = 100)]
        public virtual bool ShowProductSearchResults { get; set; }

        [CultureSpecific]
        [Display(Name = "Show contents in search results", GroupName = TabNames.SearchSettings, Order = 150)]
        public virtual bool ShowContentSearchResults { get; set; }

        [CultureSpecific]
        [Display(Name = "Show PDFs in search results", GroupName = TabNames.SearchSettings, Order = 175)]
        public virtual bool ShowPdfSearchResults { get; set; }

        [CultureSpecific]
        [Display(Name = "Include images in contents search results", GroupName = TabNames.SearchSettings, Order = 200)]
        public virtual bool IncludeImagesInContentsSearchResults { get; set; }

        [CultureSpecific]
        [SelectOne(SelectionFactoryType = typeof(CatalogSelectionFactory))]
        [Display(Name = "Search catalog", GroupName = TabNames.SearchSettings, Order = 250,
            Description = "The catalogs that will be returned by search.")]
        public virtual int SearchCatalog { get; set; }

        [CultureSpecific]
        [Display(Name = "Search Filters Configuration",
            Description = "Manage filters to be displayed on Search",
            GroupName = TabNames.SearchSettings,
            Order = 300)]
        [EditorDescriptor(EditorDescriptorType = typeof(IgnoreCollectionEditorDescriptor<FacetFilterConfigurationItem>))]
        public virtual IList<FacetFilterConfigurationItem> SearchFiltersConfiguration { get; set; }

        [SelectOne(SelectionFactoryType = typeof(CurrencySelectionFactory))]
        [Display(Name = "Currency", GroupName = TabNames.SearchSettings, Order = 210)]
        public virtual string Currency { get; set; }
    }

    public class SearchOptionSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Quick search", Value = "QuickSearch" },
                new SelectItem { Text = "Auto search", Value = "AutoSearch" }
            };
        }
    }
}