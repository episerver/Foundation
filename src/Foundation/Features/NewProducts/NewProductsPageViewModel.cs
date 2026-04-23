using Foundation.Features.CatalogContent;

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
