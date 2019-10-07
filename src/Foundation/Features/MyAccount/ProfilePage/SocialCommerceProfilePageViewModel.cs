using Foundation.Commerce.Models.Pages;
using Foundation.Commerce.ViewModels;

namespace Foundation.Features.MyAccount.ProfilePage
{
    public class SocialCommerceProfilePageViewModel : CommerceProfilePageViewModel<Social.Models.Pages.ProfilePage, Demo.ViewModels.DemoHeaderViewModel>
    {
        public SocialCommerceProfilePageViewModel()
        {

        }

        public SocialCommerceProfilePageViewModel(Social.Models.Pages.ProfilePage profilePage) : base(profilePage)
        {

        }

        public CommerceHomePage CommerceHomePage => StartPage as CommerceHomePage;
    }
}