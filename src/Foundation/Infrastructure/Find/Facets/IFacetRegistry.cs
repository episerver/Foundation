using System.Collections.Generic;

namespace Foundation.Infrastructure.Find.Facets
{
    public interface IFacetRegistry
    {
        void Clear();
        List<FacetDefinition> GetFacetDefinitions();
        void AddFacetDefinitions(FacetDefinition facetDefinition);
        bool RemoveFacetDefinitions(FacetDefinition facetDefinition);
    }
}