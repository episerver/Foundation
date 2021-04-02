using EPiServer.Find;
using EPiServer.Find.Api.Facets;
using EPiServer.Find.Api.Querying;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Find.Facets
{
    public class FacetStringListDefinition : FacetDefinition
    {
        public FacetStringListDefinition()
        {
            RenderType = GetType().Name;
            Name = "Facet Test";
            DisplayName = "Facet Test Display Name";
        }

        public ITypeSearch<T> Filter<T>(ITypeSearch<T> query, List<string> selectedValues) => selectedValues.Any() ? query.AddStringListFilter(selectedValues, FieldName) : query;

        public override ITypeSearch<T> Facet<T>(ITypeSearch<T> query, Filter filter) => SearchExtensions.TermsFacetFor(query, FieldName, null, filter);

        public override void PopulateFacet(FacetGroupOption facetGroupOption, Facet facet, string selectedFacets)
        {
            var termsFacet = facet as TermsFacet;
            if (termsFacet == null)
            {
                return;
            }

            facetGroupOption.Facets = termsFacet.Terms.Select(x => new FacetOption
            {
                Count = x.Count,
                Key = $"{facet.Name}:{x.Term}",
                Name = x.Term,
                Selected = selectedFacets != null && selectedFacets.Contains($"{facet.Name}:{x.Term}")
            }).ToList();
        }
    }
}