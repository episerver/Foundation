using EPiServer.Find.Api.Facets;
using EPiServer.Find.Api.Querying;
using Newtonsoft.Json;

namespace Foundation.Infrastructure.Find.Facets
{
    public class FacetFilterRequest : FacetRequest
    {
        public FacetFilterRequest(string name, Filter facetFilter)
            : base(name) => FacetFilter = facetFilter;

        [JsonIgnore]
        [JsonProperty("facet_filter", NullValueHandling = NullValueHandling.Ignore)]
        public Filter FacetFilter { get; set; }
    }
}