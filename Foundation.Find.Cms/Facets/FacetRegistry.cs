using System.Collections.Generic;
using System.Linq;

namespace Foundation.Find.Cms.Facets
{
    public class FacetRegistry : IFacetRegistry
    {
        private readonly List<FacetDefinition> _facetDefinitions;

        public FacetRegistry(IEnumerable<FacetDefinition> facetDefinitions) => _facetDefinitions = facetDefinitions.ToList();

        public List<FacetDefinition> GetFacetDefinitions() => _facetDefinitions;

        public void AddFacetDefinitions(FacetDefinition facetDefinition) => _facetDefinitions.Add(facetDefinition);

        public bool RemoveFacetDefinitions(FacetDefinition facetDefinition) => _facetDefinitions.Remove(facetDefinition);
    }
}