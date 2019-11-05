﻿using EPiServer;
using EPiServer.Web.Routing;
using Foundation.Cms.ViewModels.Header;
using Foundation.Demo.Models;
using Foundation.Demo.ViewModels;
using System.Web.Mvc;

namespace Foundation.Features.Header
{
    public class HeaderController : Controller
    {
        private readonly IHeaderViewModelFactory _headerViewModelFactory;
        private readonly IContentRouteHelper _contentRouteHelper;
        private readonly IContentLoader _contentLoader;

        public HeaderController(IHeaderViewModelFactory headerViewModelFactory,
            IContentRouteHelper contentRouteHelper,
            IContentLoader contentLoader)
        {
            _headerViewModelFactory = headerViewModelFactory;
            _contentRouteHelper = contentRouteHelper;
            _contentLoader = contentLoader;
        }

        [ChildActionOnly]
        public ActionResult GetHeader(DemoHomePage homePage)
        {
            var content = _contentRouteHelper.Content;
            return PartialView("_Header", _headerViewModelFactory.CreateHeaderViewModel<DemoHeaderViewModel>(content, homePage));
        }
    }
}