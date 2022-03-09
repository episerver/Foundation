using Foundation.Features.MyAccount.OrderHistory;
using Foundation.Features.Shared;
using Mediachase.Commerce.Orders;
using System.Collections.Generic;

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