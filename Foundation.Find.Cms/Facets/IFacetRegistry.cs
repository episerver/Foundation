using System.Collections.Generic;

namespace Foundation.Find.Cms.Facets
{
    public interface IFacetRegistry
    {
        List<FacetDefinition> GetFacetDefinitions();
        void AddFacetDefinitions(FacetDefinition facetDefinition);
        bool RemoveFacetDefinitions(FacetDefinition facetDefinition);
    }
}