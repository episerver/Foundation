using Foundation.Commerce.Models.Pages;

namespace Foundation.Commerce.Order.ViewModels
{
    public class SharedCartViewModel : CartViewModelBase<SharedCartPage>
    {
        public SharedCartViewModel(SharedCartPage sharedCartPage) : base(sharedCartPage)
        {
        }
    }
}