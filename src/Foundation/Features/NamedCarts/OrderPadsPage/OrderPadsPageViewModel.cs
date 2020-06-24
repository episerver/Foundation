using EPiServer.Commerce.Order;
using Foundation.Commerce.Customer;
using Foundation.Features.NamedCarts.OrderPadsPage;
using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.Checkout.ViewModels
{
    public class OrderPadsPageViewModel : ContentViewModel<OrderPadsPage>
    {
        public string QuoteStatus { get; set; }
        public FoundationContact CurrentCustomer { get; set; }
        public List<ICart> OrderPardCartsList { get; set; }
        public List<OrganizationOrderPadViewModel> OrganizationOrderPadList { get; set; }
    }
}