using EPiServer.Core;
using Foundation.Commerce.Models.Pages;

namespace Foundation.Commerce.Order.ViewModels
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