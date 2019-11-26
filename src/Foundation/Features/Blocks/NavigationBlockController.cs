using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms.Blocks;
using Foundation.Cms.ViewModels.Blocks;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Blocks
{
    [TemplateDescriptor(Default = true)]
    public class NavigationBlockController : BlockController<NavigationBlock>
    {
        private readonly IContentLoader _contentLoader;
        private readonly IPageRouteHelper _pageRouteHelper;

        public NavigationBlockController(IContentLoader contentLoader, IPageRouteHelper pageRouteHelper)
        {
            _contentLoader = contentLoader;
            _pageRouteHelper = pageRouteHelper;
        }

        public override ActionResult Index(NavigationBlock currentBlock)
        {
            var rootNavigation = currentBlock.RootPage as ContentReference;
            if (currentBlock.RootPage == null)
            {
                rootNavigation = _pageRouteHelper.ContentLink;
            }

            var childPages = _contentLoader.GetChildren<PageData>(rootNavigation);
            var model = new NavigationBlockViewModel(currentBlock);
            if (childPages != null && childPages.Count() > 0)
            {
                foreach (var page in childPages)
                {
                    model.Items.Add(new NavigationItem(page, Url));
                }
            }

            return PartialView("~/Features/Blocks/Views/NavigationBlock.cshtml", model);
        }
    }
}