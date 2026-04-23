// EPiServer.Find removed: no CMS 13 version. Replaced by Optimizely Graph (Phase 4).
// This file provides stub types so the codebase compiles without EPiServer.Find packages.

using EPiServer.Cms.Shell.UI.ObjectEditing.EditorDescriptors;
using EPiServer.Shell.ObjectEditing;

// Stubs for EPiServer.Find types used in LocationItemPage.
namespace EPiServer.Find
{
    // Stub for GeoLocation — Find's lat/lon pair used in LocationItemPage.Coordinates.
    // Graph geo-search is not implemented; coordinates are stored but not used for distance queries.
    // Note: [Ignore] attribute is NOT stubbed here — EPiServer.DataAnnotations.IgnoreAttribute (global using) is used instead.
    public class GeoLocation
    {
        public GeoLocation(double latitude, double longitude)
        {
            Latitude = latitude;
            Longitude = longitude;
        }
        public double Latitude { get; }
        public double Longitude { get; }
    }
}

namespace Foundation.Infrastructure.Find.Facets
{
    // Stub FacetDefinition: replaces EPiServer.Find-dependent abstract class.
    // Original abstract methods (Facet, PopulateFacet) removed — they took EPiServer.Find types as parameters.
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
    }

    // Stub SelectableNumericRange: replaces EPiServer.Find.Api.Facets-dependent class.
    public class SelectableNumericRange
    {
        private string _id;

        public string Id
        {
            get
            {
                if (!string.IsNullOrEmpty(_id)) return _id;
                var from = From == null ? "MIN" : From.ToString();
                var to = To == null ? "MAX" : To.ToString();
                return from + "-" + to;
            }
            set => _id = value;
        }

        public double? From { get; set; }
        public double? To { get; set; }
        public bool Selected { get; set; }
    }

    // Stub DidYouMeanResult: replaces EPiServer.Find.Statistics.Api.DidYouMeanResult.
    public class DidYouMeanResult
    {
        public int TotalMatching { get; set; }
        public IEnumerable<SuggestionHit> Hits { get; } = Enumerable.Empty<SuggestionHit>();
    }

    public class SuggestionHit
    {
        public string Suggestion { get; set; }
    }
}

namespace Foundation.Infrastructure.Find.Facets.Config
{
    // Stub EnumSelectionDescription: replaces EPiServer.Find.Helpers.EnumSelectionDescriptionAttribute
    // used on FacetDisplayMode, FacetContentFieldName, FacetFieldType, FacetDisplayDirection enums.
    [AttributeUsage(AttributeTargets.Field)]
    public class EnumSelectionDescriptionAttribute : Attribute
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }

    // Stub FacetConfigFactory: replaces Find-dependent implementation.
    // Returns empty lists — no facets configured when Find is absent.
    public class FacetConfigFactory : IFacetConfigFactory
    {
        private readonly IContentLoader _contentLoader;

        public FacetConfigFactory(IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public virtual List<FacetDefinition> GetDefaultFacetDefinitions() => new List<FacetDefinition>();

        public virtual FacetDefinition GetFacetDefinition(FacetFilterConfigurationItem facetConfiguration) => null;

        public List<FacetFilterConfigurationItem> GetFacetFilterConfigurationItems() => new List<FacetFilterConfigurationItem>();
    }
}

namespace Foundation.Features.Search
{
    // Stub UnifiedSearchHit: replaces EPiServer.Find.UnifiedSearch hit type.
    // Properties match what views and controllers access.
    public class UnifiedSearchHit
    {
        public string Url { get; set; }
        public Uri ImageUri { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string SearchSection { get; set; }
    }
}
