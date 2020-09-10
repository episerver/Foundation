using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.Category
{
    public class CategoryFoundationPageViewModel : ContentViewModel<FoundationPageData>
    {
        public CategoryFoundationPageViewModel()
        {
        }

        public CategoryFoundationPageViewModel(FoundationPageData pageData) : base(pageData)
        {
        }

        public string PreviewText { get; set; }
        public IEnumerable<StandardCategory> Categories { get; set; }
    }
}