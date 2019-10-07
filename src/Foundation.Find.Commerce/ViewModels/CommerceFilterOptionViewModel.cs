using Foundation.Find.Cms.Facets;
using Foundation.Find.Cms.ViewModels;
using System.Collections.Generic;

namespace Foundation.Find.Commerce.ViewModels
{
    public class CommerceFilterOptionViewModel : CmsFilterOptionViewModel
    {
        public List<FacetGroupOption> FacetGroups { get; set; }
        public bool SearchProduct { get; set; }
    }
}
