using Foundation.Cms.ViewModels.Header;
using Foundation.Social.Models.Pages;

namespace Foundation.Social.ViewModels
{
    public class ProfilePageViewModel<T> : Cms.ViewModels.Pages.ProfilePageViewModel<T, HeaderViewModel> where T : ProfilePage
    {
        public ProfilePageViewModel()
        {

        }

        public ProfilePageViewModel(T profilePage) : base(profilePage)
        {

        }
    }
}