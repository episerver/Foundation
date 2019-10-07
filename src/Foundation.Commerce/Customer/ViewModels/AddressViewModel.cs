using Foundation.Cms.ViewModels;
using Foundation.Commerce.Models.Pages;

namespace Foundation.Commerce.Customer.ViewModels
{
    public class AddressViewModel : ContentViewModel<AddressBookPage>
    {
        public AddressModel Address { get; set; }

        public AddressViewModel(AddressBookPage currentPage) : base(currentPage) { }
        public AddressViewModel() : base() { }
    }
}