using System.ComponentModel;
using Foundation.Infrastructure.Cms.Settings;
using Foundation.Infrastructure.Commerce.Models.EditorDescriptors;
// CMS 13 / Phase 3: SearchFiltersConfiguration removed from code model pending Phase 4 Graph replacement.
// IFacetConfiguration removed from SearchSettings for the same reason.
// The property definition remains in the DB (pkID=1007 → FacetFilterConfigurationProperty) as an orphan.

namespace Foundation.Features.Settings
{
    [SettingsContentType(DisplayName = "Search & Catalog Settings",
        GUID = "d4171337-70a4-476a-aa3c-0d976ac185e8",
        SettingsName = "Search Settings")]
    public class SearchSettings : SettingsBase
    {
        [CultureSpecific]
        [DefaultValue("QuickSearch")]
        [SelectOne(SelectionFactoryType = typeof(SearchOptionSelectionFactory))]
        [Display(Name = "Search option", GroupName = TabNames.SearchSettings, Order = 50)]
        public virtual string SearchOption { get; set; }

        [CultureSpecific]
        [DefaultValue(true)]
        [Display(Name = "Show products in search results", GroupName = TabNames.SearchSettings, Order = 100)]
        public virtual bool ShowProductSearchResults { get; set; }

        [CultureSpecific]
        [DefaultValue(true)]
        [Display(Name = "Show contents in search results", GroupName = TabNames.SearchSettings, Order = 150)]
        public virtual bool ShowContentSearchResults { get; set; }

        [CultureSpecific]
        [DefaultValue(true)]
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

        // Phase 4 TODO: re-add SearchFiltersConfiguration with Optimizely Graph backing type.
        // [BackingType(typeof(FacetFilterConfigurationProperty))]
        // public virtual IList<FacetFilterConfigurationItem> SearchFiltersConfiguration { get; set; }

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