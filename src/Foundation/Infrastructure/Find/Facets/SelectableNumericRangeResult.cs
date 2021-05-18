using EPiServer.Find.Api.Facets;

namespace Foundation.Infrastructure.Find.Facets
{
    public class SelectableNumericRangeResult : NumericRangeResult, ISelectable
    {
        private string _id;

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

        public bool Selected { get; set; }
    }
}