using Foundation.Features.CatalogContent;

namespace Foundation.Features.Sales
{
    public class SalesPageViewModel : ContentViewModel<SalesPage>
    {
        public IEnumerable<ProductTileViewModel> ProductViewModels { get; set; }

        public int PageNumber { get; set; } = 1;

        public List<int> Pages { get; set; }

        public SalesPageViewModel(SalesPage currentPage) : base(currentPage) { }
    }
}
