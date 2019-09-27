using Foundation.Cms.ViewModels;
using Foundation.Commerce.Models.Pages;
using System.Collections.Generic;

namespace Foundation.Commerce.Order.ViewModels
{
    public class OrdersPageViewModel : ContentViewModel<OrdersPage>
    {
        public List<OrderOrganizationViewModel> OrdersOrganization { get; set; }
        public string OrderDetailsPageUrl { get; set; }
    }
}