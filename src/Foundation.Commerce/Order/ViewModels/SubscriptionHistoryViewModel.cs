using Foundation.Cms.ViewModels;
using Foundation.Commerce.Models.Pages;
using Mediachase.Commerce.Orders;
using System.Collections.Generic;

namespace Foundation.Commerce.Order.ViewModels
{
    /// <summary>
    /// Model for list all payment plans
    /// </summary>
    public class SubscriptionHistoryViewModel : ContentViewModel<SubscriptionHistoryPage>
    {
        public SubscriptionHistoryViewModel(SubscriptionHistoryPage currentPage) : base(currentPage)
        {
        }

        public List<PaymentPlan> PaymentPlans { get; set; }
        public string PaymentPlanDetailsPageUrl { get; set; }
    }
}