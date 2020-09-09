using EPiServer.Core;
using EPiServer.Web.Mvc.Html;
using Foundation.Features.Shared;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.Blocks.BreadcrumbBlock
{
    public class BreadcrumbBlockViewModel : BlockViewModel<BreadcrumbBlock>
    {
        public BreadcrumbBlockViewModel(BreadcrumbBlock currentBlock) : base(currentBlock)
        {
            Breadcrumb = new List<BreadcrumbItem>();
        }

        public List<BreadcrumbItem> Breadcrumb { get; set; }
    }

    public class BreadcrumbItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public PageData PageData { get; set; }

        public BreadcrumbItem(PageData page, UrlHelper urlHelper)
        {
            if (page != null)
            {
                Name = page.Name;
                Url = urlHelper.ContentUrl(page.ContentLink);
                PageData = page;
            }
            else
            {
                Name = string.Empty;
                Url = string.Empty;
                PageData = null;
            }
        }
    }
}
