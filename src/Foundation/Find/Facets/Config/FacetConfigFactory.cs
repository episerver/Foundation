using Foundation.Cms.Settings;
using Mediachase.Commerce;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Find.Facets.Config
{
    public class FacetConfigFactory : IFacetConfigFactory
    {
        private readonly ICurrentMarket _currentMarket;
        private readonly ISettingsService _settingsService;

        public FacetConfigFactory(ICurrentMarket currentMarket,
            ISettingsService settingsService)
        {
            _currentMarket = currentMarket;
            _settingsService = settingsService;
        }

        public List<FacetDefinition> GetDefaultFacetDefinitions()
        {
            var brand = new FacetStringDefinition
            {
                FieldName = "Brand",
                DisplayName = "Brand"
            };

            var color = new FacetStringListDefinition
            {
                DisplayName = "Color",
                FieldName = "AvailableColors"
            };

            var size = new FacetStringListDefinition
            {
                DisplayName = "Size",
                FieldName = "AvailableSizes"
            };

            var priceRanges = new FacetNumericRangeDefinition(_currentMarket)
            {
                DisplayName = "Price",
                FieldName = "DefaultPrice",
                BackingType = typeof(double)
            };
            priceRanges.Range.Add(new SelectableNumericRange() { To = 50 });
            priceRanges.Range.Add(new SelectableNumericRange() { From = 50, To = 100 });
            priceRanges.Range.Add(new SelectableNumericRange() { From = 100, To = 500 });
            priceRanges.Range.Add(new SelectableNumericRange() { From = 500, To = 1000 });
            priceRanges.Range.Add(new SelectableNumericRange() { From = 1000 });

            return new List<FacetDefinition>() { priceRanges, brand, size, color };
        }
        public virtual FacetDefinition GetFacetDefinition(FacetFilterConfigurationItem facetConfiguration)
        {
            switch (Enum.Parse(typeof(FacetFieldType), facetConfiguration.FieldType))
            {
                case FacetFieldType.String:
                    return new FacetStringDefinition
                    {
                        FieldName = facetConfiguration.FieldName,
                        DisplayName = facetConfiguration.GetDisplayName()
                    };

                case FacetFieldType.ListOfString:
                    return new FacetStringListDefinition
                    {
                        FieldName = facetConfiguration.FieldName,
                        DisplayName = facetConfiguration.GetDisplayName()
                    };

                case FacetFieldType.Boolean:
                case FacetFieldType.NullableBoolean:
                    return new FacetStringListDefinition
                    {
                        FieldName = facetConfiguration.FieldName,
                        DisplayName = facetConfiguration.GetDisplayName(),
                    };
                case FacetFieldType.Integer:
                    return new FacetNumericRangeDefinition(_currentMarket)
                    {
                        FieldName = facetConfiguration.FieldName,
                        DisplayName = facetConfiguration.GetDisplayName(),
                        BackingType = typeof(int)
                    };

                case FacetFieldType.Double:
                    if (facetConfiguration.DisplayMode == FacetDisplayMode.Range.ToString()
                        || facetConfiguration.DisplayMode == FacetDisplayMode.PriceRange.ToString())
                    {
                        var rangeDefinition = new FacetNumericRangeDefinition(_currentMarket)
                        {
                            FieldName = facetConfiguration.FieldName,
                            DisplayName = facetConfiguration.GetDisplayName(),
                            BackingType = typeof(double)
                        };

                        rangeDefinition.Range = facetConfiguration.GetSelectableNumericRanges();

                        return rangeDefinition;
                    }
                    else if (facetConfiguration.DisplayMode == FacetDisplayMode.Rating.ToString())
                    {
                        var rangeDefinition = new FacetAverageRatingDefinition(_currentMarket)
                        {
                            FieldName = facetConfiguration.FieldName,
                            DisplayName = facetConfiguration.GetDisplayName(),
                            BackingType = typeof(double)
                        };

                        rangeDefinition.Range = facetConfiguration.GetSelectableNumericRanges();

                        return rangeDefinition;
                    }
                    break;
            }

            return new FacetStringDefinition
            {
                FieldName = facetConfiguration.FieldName,
                DisplayName = facetConfiguration.GetDisplayName(),
            };
        }

        public List<FacetFilterConfigurationItem> GetFacetFilterConfigurationItems<T>() where T : SettingsBase
        {
            var settings = _settingsService.GetSiteSettings<T>();
            if (settings == null || !(settings is IFacetConfiguration facetConfiguration))
            {
                return new List<FacetFilterConfigurationItem>();
            }

            return facetConfiguration.SearchFiltersConfiguration.ToList();
        }
    }
}
