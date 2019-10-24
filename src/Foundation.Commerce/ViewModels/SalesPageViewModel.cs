using Foundation.Cms.ViewModels;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Commerce.Models.Pages;
using System.Collections.Generic;

namespace Foundation.Commerce.ViewModels
{
    public class SalesPageViewModel : ContentViewModel<SalesPage>
    {
        public IEnumerable<ProductTileViewModel> ProductViewModels { get; set; }

        public int PageNumber { get; set; } = 1;

        public List<int> Pages { get; set; }

        public SalesPageViewModel(SalesPage currentPage) : base(currentPage) { }
    }
}
