using EPiServer.Core;
using Foundation.Features.NamedCarts.SharedCart;

namespace Foundation.Features.Checkout.ViewModels
{
    public class SharedMiniCartViewModel : CartViewModelBase<SharedCartPage>
    {
        public SharedMiniCartViewModel(SharedCartPage sharedCartPage) : base(sharedCartPage)
        {
        }

        public ContentReference SharedCartPage { get; set; }
    }
}