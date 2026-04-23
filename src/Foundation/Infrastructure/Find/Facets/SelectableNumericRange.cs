using EPiServer.Find.Api.Facets;
using Newtonsoft.Json;

namespace Foundation.Infrastructure.Find.Facets
{
    public class SelectableNumericRange : ISelectable
    {
        private string _id;

        public SelectableNumericRange()
        {
        }

        public SelectableNumericRange(NumericRange numericRange)
        {
            From = numericRange.From;
            To = numericRange.To;
        }

        public string Id
        {
            get
            {
                if (!string.IsNullOrEmpty(_id))
                {
                    return _id;
                }

                var from = From == null ? "MIN" : From.ToString();
                var to = To == null ? "MAX" : To.ToString();
                return from + "-" + to;
            }
            set => _id = value;
        }

        [JsonProperty("from", NullValueHandling = NullValueHandling.Ignore)]
        public double? From { get; set; }

        [JsonProperty("to", NullValueHandling = NullValueHandling.Ignore)]
        public double? To { get; set; }

        public bool Selected { get; set; }

        public NumericRange ToNumericRange() => new NumericRange(From, To);
    }
}