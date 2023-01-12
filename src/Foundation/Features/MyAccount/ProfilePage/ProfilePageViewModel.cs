using Foundation.Features.Checkout.ViewModels;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Infrastructure.Cms.Users;
using Foundation.Infrastructure.Commerce.Customer;

namespace Foundation.Features.MyAccount.ProfilePage
{
    public class ProfilePageViewModel : ContentViewModel<ProfilePage>
    {
        public ProfilePageViewModel()
        {
        }

        public ProfilePageViewModel(ProfilePage profilePage) : base(profilePage)
        {
        }

        public List<OrderViewModel> Orders { get; set; }
        public IEnumerable<AddressModel> Addresses { get; set; }
        public SiteUser SiteUser { get; set; }
        public FoundationContact CustomerContact { get; set; }
        public string OrderDetailsPageUrl { get; set; }
        public string ResetPasswordPage { get; set; }
        public string AddressBookPage { get; set; }
    }
}