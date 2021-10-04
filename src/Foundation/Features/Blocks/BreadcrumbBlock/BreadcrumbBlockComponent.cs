using EPiServer;
using EPiServer.Core;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Foundation.Features.Blocks.BreadcrumbBlock
{
    public class BreadcrumbBlockComponent : AsyncBlockComponent<BreadcrumbBlock>
    {
        private readonly IContentLoader _contentLoader;
        private readonly IPageRouteHelper _pageRouteHelper;

        public BreadcrumbBlockComponent(IContentLoader contentLoader, IPageRouteHelper pageRouteHelper)
        {
            _contentLoader = contentLoader;
            _pageRouteHelper = pageRouteHelper;
        }

        protected override async Task<IViewComponentResult> InvokeComponentAsync(BreadcrumbBlock currentBlock)
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
                var breadcrumb = new List<BreadcrumbItem>();

                foreach (var page in ancestors)
                {
                    breadcrumb.Add(new BreadcrumbItem(page, Url));
                }

                breadcrumb.Add(new BreadcrumbItem(_contentLoader.Get<IContent>(destination) as PageData, Url));
                model.Breadcrumb.AddRange(breadcrumb.Where(x => !string.IsNullOrEmpty(x.Url)));
            }

            return await Task.FromResult(View("~/Features/Blocks/BreadcrumbBlock/BreadcrumbBlock.cshtml", model));
        }
    }
}