using EPiServer;
using EPiServer.Core;
using Mediachase.Commerce;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Find.Facets.Config
{
    public class FacetConfigFactory : IFacetConfigFactory
    {
        private readonly IContentLoader _contentLoader;
        private readonly ICurrentMarket _currentMarket;

        public FacetConfigFactory(IContentLoader contentLoader,
            ICurrentMarket currentMarket)
        {
            _contentLoader = contentLoader;
            _currentMarket = currentMarket;
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

        public List<FacetFilterConfigurationItem> GetFacetFilterConfigurationItems()
        {
            if (ContentReference.IsNullOrEmpty(ContentReference.StartPage))
            {
                return new List<FacetFilterConfigurationItem>();
            }

            var startPage = _contentLoader.Get<IContent>(ContentReference.StartPage);

            var facetsConfiguration = startPage as IFacetConfiguration;
            if (facetsConfiguration?.SearchFiltersConfiguration != null)
            {
                return facetsConfiguration
                    .SearchFiltersConfiguration
                    .ToList();
            }

            return new List<FacetFilterConfigurationItem>();
        }
    }
}
