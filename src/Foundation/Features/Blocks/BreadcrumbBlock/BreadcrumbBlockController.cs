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

            if (ancestors != null && ancestors.Any())
            {
                var breadcrumb = new List<BreadcrumbItem>();

                foreach (var page in ancestors)
                {
                    breadcrumb.Add(new BreadcrumbItem(page, Url));
                }

                breadcrumb.Add(new BreadcrumbItem(_contentLoader.Get<IContent>(destination) as PageData, Url));
                model.Breadcrumb.AddRange(breadcrumb.Where(x => !string.IsNullOrEmpty(x.Url)));
            }

            return PartialView("~/Features/Blocks/BreadcrumbBlock/BreadcrumbBlock.cshtml", model);
        }
    }
}