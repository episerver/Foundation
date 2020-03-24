using EPiServer.Core;
using EPiServer.Web.Routing;
using Foundation.Demo.Interfaces;
using Foundation.Demo.Models;
using System.Web.Mvc;

namespace Foundation.Features.MultiLanguages
{
    public class MultiLanguagesController : Controller
    {
        private readonly IDemoFooterViewModelFactory _demoFooterViewModelFactory;
        private readonly IContentRouteHelper _contentRouteHelper;

        public MultiLanguagesController(IDemoFooterViewModelFactory demoFooterViewModelFactory,
            IContentRouteHelper contentRouteHelper)
        {
            _demoFooterViewModelFactory = demoFooterViewModelFactory;
            _contentRouteHelper = contentRouteHelper;
        }

        [ChildActionOnly]
        public ActionResult GetFooterMultiLanguages(DemoHomePage homePage)
        {
            var currentPage = _contentRouteHelper.Content as PageData;
            var model = _demoFooterViewModelFactory.CreateDemoFooterViewModel(homePage, currentPage);
            return PartialView("FooterMultiLanguage", model);
        }

        [ChildActionOnly]
        public ActionResult GetAlternateTags()
        {
            var currentPage = _contentRouteHelper.Content as PageData;
            var model = _demoFooterViewModelFactory.GetCurrentPageLanguages(currentPage);
            return PartialView("AlternateTags", model);
        }
    }
}