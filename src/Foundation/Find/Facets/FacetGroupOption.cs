using System.Collections.Generic;

namespace Foundation.Find.Facets
{
    public class FacetGroupOption
    {
        public string GroupName { get; set; }
        public List<FacetOption> Facets { get; set; }
        public string GroupFieldName { get; set; }
    }
}