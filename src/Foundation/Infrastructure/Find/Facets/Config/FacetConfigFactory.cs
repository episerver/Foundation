namespace Foundation.Infrastructure.Find.Facets.Config
{
    public class FacetConfigFactory : IFacetConfigFactory
    {
        private readonly IContentLoader _contentLoader;

        public FacetConfigFactory(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public virtual List<FacetDefinition> GetDefaultFacetDefinitions()
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

            var priceRanges = new FacetNumericRangeDefinition(ServiceLocator.Current.GetInstance<ICurrentMarket>())
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
