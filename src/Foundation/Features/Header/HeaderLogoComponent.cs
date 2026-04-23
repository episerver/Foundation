using Foundation.Features.Home;

namespace Foundation.Features.Header
{
    public class HeaderLogoComponent : ViewComponent
    {
        private readonly IHeaderViewModelFactory _headerViewModelFactory;
        private readonly IContentRouteHelper _contentRouteHelper;

        public HeaderLogoComponent(IHeaderViewModelFactory headerViewModelFactory,
            IContentRouteHelper contentRouteHelper)
        {
            _headerViewModelFactory = headerViewModelFactory;
            _contentRouteHelper = contentRouteHelper;
        }

        public IViewComponentResult Invoke(HomePage homePage)
        {
            var content = _contentRouteHelper.Content;
            return View("_HeaderLogo", _headerViewModelFactory.CreateHeaderViewModel(content, homePage));
        }
    }
}
