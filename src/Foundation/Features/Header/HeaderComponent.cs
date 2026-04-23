using Foundation.Features.Home;

namespace Foundation.Features.Header
{
    public class HeaderComponent : ViewComponent
    {
        private readonly IHeaderViewModelFactory _headerViewModelFactory;
        private readonly IContentRouteHelper _contentRouteHelper;

        public HeaderComponent(IHeaderViewModelFactory headerViewModelFactory,
            IContentRouteHelper contentRouteHelper)
        {
            _headerViewModelFactory = headerViewModelFactory;
            _contentRouteHelper = contentRouteHelper;
        }

        public IViewComponentResult Invoke(HomePage homePage)
        {
            var content = _contentRouteHelper.Content;
            return View("_Header", _headerViewModelFactory.CreateHeaderViewModel(content, homePage));
        }
    }
}
