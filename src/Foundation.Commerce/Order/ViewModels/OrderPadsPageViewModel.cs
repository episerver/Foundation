using EPiServer.Commerce.Order;
using Foundation.Cms.ViewModels;
using Foundation.Commerce.Customer;
using Foundation.Commerce.Models.Pages;
using System.Collections.Generic;

namespace Foundation.Commerce.Order.ViewModels
{
    public class OrderPadsPageViewModel : ContentViewModel<OrderPadsPage>
    {
        public string QuoteStatus { get; set; }
        public FoundationContact CurrentCustomer { get; set; }
        public List<ICart> OrderPardCartsList { get; set; }
        public List<OrganizationOrderPadViewModel> OrganizationOrderPadList { get; set; }
    }
}