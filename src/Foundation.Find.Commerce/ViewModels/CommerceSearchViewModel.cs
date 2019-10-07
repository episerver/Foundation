using EPiServer.Core;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Find.Cms.ViewModels;
using System.Collections.Generic;

namespace Foundation.Find.Commerce.ViewModels
{
    public class CommerceSearchViewModel<T> : CmsSearchViewModel<T> where T : IContent
    {
        public CommerceSearchViewModel()
        {

        }

        public CommerceSearchViewModel(T currentContent) : base(currentContent)
        {

        }

        public IEnumerable<ProductTileViewModel> ProductViewModels { get; set; }
        public CategoriesFilterViewModel CategoriesFilter { get; set; }
        public List<KeyValuePair<string, string>> BreadCrumb { get; set; }
    }
}
