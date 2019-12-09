using EPiServer;
using Foundation.Find.Cms.Facets;
using Foundation.Find.Cms.Facets.Config;
using Mediachase.Commerce;
using System;
using System.Collections.Generic;

namespace Foundation.Find.Commerce.Facets
{
    public class CommerceFacetConfigFactory : FacetConfigFactory
    {
        private readonly ICurrentMarket _currentMarket;

        public CommerceFacetConfigFactory(ICurrentMarket currentMarket,
            IContentLoader contentLoader) : base(contentLoader)
        {
            _currentMarket = currentMarket;
        }

        public override FacetDefinition GetFacetDefinition(FacetFilterConfigurationItem facetConfiguration)
        {
            switch (Enum.Parse(typeof(FacetFieldType), facetConfiguration.FieldType))
            {
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

            return base.GetFacetDefinition(facetConfiguration);

        }

        public override List<FacetDefinition> GetDefaultFacetDefinitions()
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
    }
}
