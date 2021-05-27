using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Api.Querying;
using EPiServer.Framework.Localization;

namespace Foundation.Find.Facets
{
    public abstract class FacetDefinition
    {
        private string _displayName;

        public string Name { get; set; }

        public string DisplayName
        {
            get => !string.IsNullOrEmpty(FieldName)
                ? LocalizationService.Current.GetString("/facetregistry/" + FieldName.ToLowerInvariant(),
                    !string.IsNullOrEmpty(_displayName) ? _displayName : FieldName)
                : _displayName;

            set => _displayName = value;
        }

        public string FieldName { get; set; }
        public string RenderType { get; set; }

        public abstract ITypeSearch<T> Facet<T>(ITypeSearch<T> query, Filter filter);
        public abstract void PopulateFacet(FacetGroupOption facetGroupOption, Facet facet, string selectedFacets);
    }
}