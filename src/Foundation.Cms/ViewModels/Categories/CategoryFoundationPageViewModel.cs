using Foundation.Cms.Categories;
using Foundation.Cms.Pages;
using System.Collections.Generic;

namespace Foundation.Cms.ViewModels.Categories
{
    public class CategoryFoundationPageViewModel : ContentViewModel<FoundationPageData>
    {
        public CategoryFoundationPageViewModel() { }
        public CategoryFoundationPageViewModel(FoundationPageData pageData) : base(pageData) { }

        public string PreviewText { get; set; }
        public IEnumerable<StandardCategory> Categories { get; set; }
    }
}
