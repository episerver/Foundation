using Foundation.Cms.ViewModels;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Commerce.Models.Pages;

namespace Foundation.Commerce.ViewModels
{
    public class StorePageViewModel : ContentViewModel<StorePage>
    {
        public StoreViewModel StoreViewModel { get; set; }

        public StorePageViewModel(StorePage currentPage) : base(currentPage) { }
    }
}