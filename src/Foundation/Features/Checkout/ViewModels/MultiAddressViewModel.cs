using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.Checkout.ViewModels
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