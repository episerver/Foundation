using Foundation.Cms.ViewModels;
using Foundation.Commerce.Models.Pages;
using System.Collections.Generic;

namespace Foundation.Commerce.Customer.ViewModels
{
    public class AddressCollectionViewModel : ContentViewModel<AddressBookPage>
    {
        public AddressCollectionViewModel()
        {
        }

        public AddressCollectionViewModel(AddressBookPage currentPage) : base(currentPage) { }

        public IEnumerable<AddressModel> Addresses { get; set; }
    }
}