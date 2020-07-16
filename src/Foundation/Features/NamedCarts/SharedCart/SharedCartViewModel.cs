using Foundation.Features.NamedCarts.SharedCart;

namespace Foundation.Features.Checkout.ViewModels
{
    public class SharedCartViewModel : CartViewModelBase<SharedCartPage>
    {
        public SharedCartViewModel(SharedCartPage sharedCartPage) : base(sharedCartPage)
        {
        }
    }
}