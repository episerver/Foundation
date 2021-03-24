using EPiServer.Find.Helpers;
using EPiServer.Find.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Linq;

namespace Foundation.Find.Facets
{
    public class RangeFacetRequestConverter : CustomWriteConverterBase<RangeFacetFilterRequest>
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value.IsNull())
            {
                writer.WriteNull();
                return;
            }

            var facetRequest = (RangeFacetFilterRequest)value;
            writer.WriteStartObject();
            writer.WritePropertyName("range");
            writer.WriteStartObject();
            WriteNonIgnoredProperties(writer, value, serializer);
            writer.WriteEndObject();
            if (facetRequest.FacetFilter.IsNotNull())
            {
                var contract = (JsonObjectContract)serializer.ContractResolver.ResolveContract(value.GetType());
                var property = contract.Properties.FirstOrDefault(x => x.PropertyName.Equals("facet_filter"));
                if (property != null)
                {
                    WriteNonIgnoredProperty(serializer, property, facetRequest.FacetFilter, writer);
                }
            }

            writer.WriteEndObject();
        }
    }
}