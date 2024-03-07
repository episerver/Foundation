namespace Foundation.Features.MyAccount.AddressBook
{
    public class AddressViewModel : ContentViewModel<AddressBookPage>
    {
        public AddressModel Address { get; set; }

        public AddressViewModel(AddressBookPage currentPage) : base(currentPage) { }
        public AddressViewModel() : base() { }
        public int ContentReference { get; set; }
    }
}