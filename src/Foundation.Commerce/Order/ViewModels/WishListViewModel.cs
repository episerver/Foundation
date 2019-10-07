using Foundation.Commerce.Models.Pages;

namespace Foundation.Commerce.Order.ViewModels
{
    public class WishListViewModel : CartViewModelBase<WishListPage>
    {
        public WishListViewModel(WishListPage wishListPage) : base(wishListPage)
        {
        }
    }
}