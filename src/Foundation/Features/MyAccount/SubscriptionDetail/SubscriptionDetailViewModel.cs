using Foundation.Features.MyAccount.OrderHistory;
using Mediachase.Commerce.Orders;

namespace Foundation.Features.MyAccount.SubscriptionDetail
{
    public class SubscriptionDetailViewModel : ContentViewModel<SubscriptionDetailPage>
    {
        public SubscriptionDetailViewModel(SubscriptionDetailPage currentPage) : base(currentPage)
        {
        }

        public OrderHistoryViewModel Orders { get; set; }
        public PaymentPlan PaymentPlan { get; set; }
        public List<KeyValuePair<string, string>> SubscriptionOptions { get; set; }
        public string SelectedSubscriptionOption { get; set; }
    }
}