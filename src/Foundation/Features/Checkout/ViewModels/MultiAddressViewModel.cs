using Foundation.Features.MyAccount.AddressBook;

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