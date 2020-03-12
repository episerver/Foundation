using Foundation.Cms.ViewModels;
using Foundation.Commerce.Models.Pages;
using Mediachase.Commerce.Orders;

namespace Foundation.Commerce.Order.ViewModels
{
    public class SubscriptionDetailViewModel : ContentViewModel<SubscriptionDetailPage>
    {
        public SubscriptionDetailViewModel(SubscriptionDetailPage currentPage) : base(currentPage)
        {
        }

        public OrderHistoryViewModel Orders { get; set; }
        public PaymentPlan PaymentPlan { get; set; }
    }
}