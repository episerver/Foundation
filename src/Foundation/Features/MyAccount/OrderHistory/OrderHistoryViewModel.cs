using Foundation.Cms;
using Foundation.Commerce.Customer;
using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.Shared;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.MyAccount.OrderHistory
{
    public class OrderHistoryViewModel : ContentViewModel<OrderHistoryPage>
    {
        public List<OrderViewModel> Orders { get; set; }
        public string OrderDetailsPageUrl { get; set; }
        public FoundationContact CurrentCustomer { get; set; }

        public int CycleMode { get; set; }
        public int CycleLength { get; set; }
        public OrderHistoryBlock.OrderHistoryBlock CurrentBlock { get; set; }

        public List<SelectListItem> Modes => new List<SelectListItem>
        {
            new SelectListItem { Text = "Every x Days", Value = "1"},
            new SelectListItem { Text = "Every x Weeks", Value = "2"},
            new SelectListItem { Text = "Every x Months", Value = "3"},
            new SelectListItem { Text = "Every x Years", Value = "4"}
        };

        public PagingInfo PagingInfo { get; set; }
        public OrderFilter Filter { get; set; }
        public string OrderHistoryUrl { get; set; }

        public OrderHistoryViewModel() : base() { }
        public OrderHistoryViewModel(OrderHistoryPage currentContent) : base(currentContent)
        {
            PagingInfo = new PagingInfo();
            Filter = new OrderFilter();
        } // currentContent must be OrderHistoryPage or OrderHistoryBlock
    }
}