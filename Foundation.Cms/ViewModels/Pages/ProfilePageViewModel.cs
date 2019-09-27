using Foundation.Cms.Pages;

namespace Foundation.Cms.ViewModels.Pages
{
    public class ProfilePageViewModel<TContent, THeaderViewModel> : ContentViewModel<TContent>
        where TContent : ProfilePage
    {
        public ProfilePageViewModel()
        {

        }

        public ProfilePageViewModel(TContent profilePage) : base(profilePage)
        {

        }
    }
}