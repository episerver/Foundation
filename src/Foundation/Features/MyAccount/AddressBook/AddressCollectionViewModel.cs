using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.MyAccount.AddressBook
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