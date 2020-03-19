using Foundation.Cms.ViewModels;
using Foundation.Commerce.Customer.ViewModels;
using Foundation.Commerce.Models.Pages;
using System.Collections.Generic;

namespace Foundation.Commerce.Order.ViewModels
{
    public class MultiAddressViewModel : ContentViewModel<CheckoutPage>
    {
        public MultiAddressViewModel()
        {
        }

        public MultiAddressViewModel(CheckoutPage multiShipmentPage) : base(multiShipmentPage)
        {
        }

        public IList<AddressModel> AvailableAddresses { get; set; }

        public CartItemViewModel[] CartItems { get; set; }

        public string ReferrerUrl { get; set; }
    }
}