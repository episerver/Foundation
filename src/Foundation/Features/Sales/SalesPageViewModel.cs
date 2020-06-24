using Foundation.Features.CatalogContent;
using Foundation.Features.Shared;
using System.Collections.Generic;

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
