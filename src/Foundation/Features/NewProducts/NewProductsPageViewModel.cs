using Foundation.Features.CatalogContent;
using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.NewProducts
{
    public class NewProductsPageViewModel : ContentViewModel<NewProductsPage>
    {
        public IEnumerable<ProductTileViewModel> ProductViewModels { get; set; }

        public int PageNumber { get; set; } = 1;

        public List<int> Pages { get; set; }

        public NewProductsPageViewModel(NewProductsPage currentPage) : base(currentPage) { }
    }
}
