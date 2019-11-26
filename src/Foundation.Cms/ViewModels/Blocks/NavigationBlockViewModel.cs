using EPiServer.Core;
using EPiServer.Web.Mvc.Html;
using Foundation.Cms.Blocks;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Cms.ViewModels.Blocks
{
    public class NavigationBlockViewModel : BlockViewModel<NavigationBlock>
    {
        public NavigationBlockViewModel(NavigationBlock currentBlock) : base(currentBlock) => Items = new List<NavigationItem>();

        public List<NavigationItem> Items { get; set; }
    }

    public class NavigationItem
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public PageData PageData { get; set; }

        public NavigationItem(PageData page, UrlHelper urlHelper)
        {
            Name = page.Name;
            Url = urlHelper.ContentUrl(page.ContentLink);
            PageData = page;
        }
    }
}
