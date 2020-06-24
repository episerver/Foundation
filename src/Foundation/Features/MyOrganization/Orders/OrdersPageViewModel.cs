using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.MyOrganization.Orders
{
    public class OrdersPageViewModel : ContentViewModel<OrdersPage>
    {
        public List<OrderOrganizationViewModel> OrdersOrganization { get; set; }
        public string OrderDetailsPageUrl { get; set; }
    }
}