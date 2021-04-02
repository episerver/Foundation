using EPiServer.Find.Api.Querying;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Foundation.Find.Facets
{
    [JsonConverter(typeof(TermsFacetFilterRequestConverter))]
    public class TermsFacetFilterRequest : FacetFilterRequest
    {
        public TermsFacetFilterRequest(string name, Filter facetFilter)
            : base(name, facetFilter)
        {
        }

        [JsonProperty("field", NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; set; }

        [JsonIgnore] public IEnumerable<string> Fields { get; set; }

        [JsonIgnore] public int? Size { get; set; }

        [JsonProperty("script", NullValueHandling = NullValueHandling.Ignore)]
        public string Script { get; set; }

        [JsonIgnore] public bool AllTerms { get; set; }
    }
}