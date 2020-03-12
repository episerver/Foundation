using EPiServer.Core;
using Foundation.Commerce.Models.Pages;

namespace Foundation.Commerce.Order.ViewModels
{
    public class SharedMiniCartViewModel : CartViewModelBase<SharedCartPage>
    {
        public SharedMiniCartViewModel(SharedCartPage sharedCartPage) : base(sharedCartPage)
        {
        }

        public ContentReference SharedCartPage { get; set; }
    }
}