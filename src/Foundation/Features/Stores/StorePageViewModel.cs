using Foundation.Features.Shared;

namespace Foundation.Features.Stores
{
    public class StorePageViewModel : ContentViewModel<StorePage>
    {
        public StoreViewModel StoreViewModel { get; set; }

        public StorePageViewModel(StorePage currentPage) : base(currentPage) { }
    }
}