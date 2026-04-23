using Foundation.Features.NamedCarts.Wishlist;

namespace Foundation.Features.Checkout.ViewModels
{
    public class WishListViewModel : CartViewModelBase<WishListPage>
    {
        public WishListViewModel(WishListPage wishListPage) : base(wishListPage)
        {
        }
    }
}