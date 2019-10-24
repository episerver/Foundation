using Foundation.Cms.ViewModels;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Commerce.Models.Pages;
using System.Collections.Generic;

namespace Foundation.Commerce.ViewModels
{
    public class NewProductsPageViewModel : ContentViewModel<NewProductsPage>
    {
        public IEnumerable<ProductTileViewModel> ProductViewModels { get; set; }

        public int PageNumber { get; set; } = 1;

        public List<int> Pages { get; set; }

        public NewProductsPageViewModel(NewProductsPage currentPage) : base(currentPage) { }
    }
}
