using EPiServer.Find.Api;
using EPiServer.Find.Helpers;
using EPiServer.Find.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using System.Linq;

namespace Foundation.Infrastructure.Find.Facets
{
    public class TermsFacetFilterRequestConverter : CustomWriteConverterBase<TermsFacetFilterRequest>
    {
        private const int MinSize = 0;
        private const int MaxSize = 1000;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var facetRequest = (TermsFacetFilterRequest)value;
            writer.WriteStartObject();
            writer.WritePropertyName("terms");
            writer.WriteStartObject();
            WriteNonIgnoredProperties(writer, value, serializer);
            if (facetRequest.Fields.IsNotNull() && facetRequest.Fields.Any())
            {
                writer.WritePropertyName("fields");
                WriteArrayValues(writer, facetRequest.Fields, serializer);
            }

            if (facetRequest.AllTerms)
            {
                writer.WritePropertyName("all_terms");
                writer.WriteValue(true);
            }

            if (facetRequest.Size.HasValue)
            {
                if (facetRequest.Size.Value < MinSize)
                {
                    throw new InvalidSearchRequestException(string.Format(CultureInfo.InvariantCulture,
                        "Terms facet size can not be set to a lower value than 0. Current value: '{0}'",
                        facetRequest.Size.Value));
                }

                if (facetRequest.Size.Value > MaxSize)
                {
                    throw new InvalidSearchRequestException(string.Format(CultureInfo.InvariantCulture,
                        "Terms facet size can not be set to a higher value than 1000. Current value: '{0}'",
                        facetRequest.Size.Value));
                }

                writer.WritePropertyName("size");
                writer.WriteValue(facetRequest.Size.Value);
            }

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