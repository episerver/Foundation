using EPiServer.Core;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.NamedCarts.Wishlist;

namespace Foundation.Features.Header
{
    public class WishListMiniCartViewModel : CartViewModelBase<WishListPage>
    {
        public WishListMiniCartViewModel(WishListPage wishListPage) : base(wishListPage)
        {
        }

        public ContentReference WishListPage { get; set; }

        public string Label { get; set; }
    }
}