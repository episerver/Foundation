using EPiServer.Find.Api.Facets;

namespace Foundation.Find.Cms.Facets
{
    public class MultiSelectTermCount : TermCount, ISelectable
    {
        public bool Selected { get; set; }
    }
}