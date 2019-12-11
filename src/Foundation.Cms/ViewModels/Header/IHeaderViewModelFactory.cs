using EPiServer.Core;
using Foundation.Cms.Pages;

namespace Foundation.Cms.ViewModels.Header
{
    public interface IHeaderViewModelFactory
    {
        THeaderViewModel CreateHeaderViewModel<THeaderViewModel>(IContent content,
            CmsHomePage home)
            where THeaderViewModel : HeaderViewModel, new();

        void AddMyAccountMenu<THomePage, THeaderViewModel>(THomePage homePage,
            THeaderViewModel viewModel)
            where THeaderViewModel : HeaderViewModel, new()
            where THomePage : CmsHomePage;
    }
}
