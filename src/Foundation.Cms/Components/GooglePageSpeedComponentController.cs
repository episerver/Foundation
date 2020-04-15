using EPiServer.Cms.Shell.ViewComposition;
using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Globalization;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Foundation.Cms.ViewModels.Components;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace Foundation.Cms.Components
{
    [IFrameComponent(
        Url = "/GooglePageSpeedComponent",
        Title = "Google Page Speed",
        Description = "A component that shows information about the Google Page Speed",
        Categories = "dashboard,cms",
        MinHeight = 500,
        MaxHeight = 1000)]
    public class GooglePageSpeedComponentController : Controller
    {
        private readonly UrlResolver _urlResolver;
        private readonly IPageRouteHelper _pageRouteHelper;

        public GooglePageSpeedComponentController(UrlResolver urlResolver, IPageRouteHelper pageRouteHelper)
        {
            _urlResolver = urlResolver;
            _pageRouteHelper = pageRouteHelper;
        }
        public ActionResult Index()
        {
            var requestContext = new RequestContext()
            {
                RouteData = new RouteData()
            };
            var id = HttpContext.Request.QueryString["id"]?.Split('_')[0];
            var contentId = !string.IsNullOrEmpty(id)
                                    ? id
                                    : _pageRouteHelper.Page.ContentLink.ID.ToString();
            requestContext.RouteData.DataTokens["contextmode"] = ContextMode.Default;
            var url = _urlResolver.GetVirtualPath(new ContentReference(contentId), ContentLanguage.PreferredCulture.Name, null, requestContext).GetUrl();
            var model = new GooglePageSpeedViewModel
            {
                Url = url
            };
            return View(model);
        }
    }
}
