using Foundation.Find.Cms.ViewModels;

namespace Foundation.Find.Commerce.ViewModels
{
    public class CommerceArgs : IArgs
    {
        public string SelectedFacets { get; set; }
        public int CatalogId { get; set; }
        public CommerceFilterOptionViewModel FilterOption { get; set; }
    }
}
