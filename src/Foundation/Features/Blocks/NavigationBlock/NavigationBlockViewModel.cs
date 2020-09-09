using EPiServer.Core;
using EPiServer.Web.Mvc.Html;
using Foundation.Features.Shared;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Features.Blocks.NavigationBlock
{
    public class NavigationBlockViewModel : BlockViewModel<NavigationBlock>
    {
        public NavigationBlockViewModel(NavigationBlock currentBlock) : base(currentBlock)
        {
            Items = new List<NavigationItem>();
            Heading = currentBlock.Heading;
        }

        public List<NavigationItem> Items { get; set; }
        public string Heading { get; set; }
    }

    public class NavigationItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public PageData PageData { get; set; }

        public NavigationItem(PageData page, UrlHelper urlHelper)
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
