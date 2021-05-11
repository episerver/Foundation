using EPiServer.Find.Api.Facets;

namespace Foundation.Infrastructure.Find.Facets
{
    public class MultiSelectTermCount : TermCount, ISelectable
    {
        public bool Selected { get; set; }
    }
}