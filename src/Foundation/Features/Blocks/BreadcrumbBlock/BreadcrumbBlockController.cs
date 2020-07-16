using EPiServer;
using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Blocks.BreadcrumbBlock
{
    [TemplateDescriptor(Default = true)]
    public class BreadcrumbBlockController : BlockController<BreadcrumbBlock>
    {
        private readonly IContentLoader _contentLoader;
        private readonly IPageRouteHelper _pageRouteHelper;

        public BreadcrumbBlockController(IContentLoader contentLoader, IPageRouteHelper pageRouteHelper)
        {
            _contentLoader = contentLoader;
            _pageRouteHelper = pageRouteHelper;
        }

        public override ActionResult Index(BreadcrumbBlock currentBlock)
        {
            var destination = currentBlock.DestinationPage as ContentReference;
            if (ContentReference.IsNullOrEmpty(currentBlock.DestinationPage))
            {
                destination = _pageRouteHelper.ContentLink;
            }

            var ancestors = _contentLoader.GetAncestors(destination).Where(x => x is PageData).Select(x => x as PageData).Reverse();
            var model = new BreadcrumbBlockViewModel(currentBlock);

            if (ancestors != null && ancestors.Count() > 0)
            {
                var breadcrumb = new List<NavigationItem>();

                foreach (var page in ancestors)
                {
                    breadcrumb.Add(new NavigationItem(page, Url));
                }

                breadcrumb.Add(new NavigationItem(_contentLoader.Get<IContent>(destination) as PageData, Url));
                model.Breadcrumb.AddRange(breadcrumb.Where(x => !string.IsNullOrEmpty(x.Url)));
            }

            return PartialView("~/Features/Blocks/BreadcrumbBlock/BreadcrumbBlock.cshtml", model);
        }
    }
}