using Foundation.Cms.ViewModels;
using Foundation.Commerce.Catalog.ViewModels;
using Foundation.Commerce.Models.Pages;
using System.Collections.Generic;

namespace Foundation.Commerce.Order.ViewModels
{
    public class QuickOrderPageViewModel : ContentViewModel<QuickOrderPage>
    {
        public List<ProductViewModel> ProductsList { get; set; }
        public List<string> ReturnedMessages { get; set; }
    }
}