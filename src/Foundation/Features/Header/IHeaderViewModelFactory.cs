using EPiServer.Core;
using Foundation.Features.Home;

namespace Foundation.Features.Header
{
    public interface IHeaderViewModelFactory
    {
        HeaderViewModel CreateHeaderViewModel(IContent content, HomePage home);
        void AddMyAccountMenu(HomePage homePage, HeaderViewModel viewModel);
    }
}
