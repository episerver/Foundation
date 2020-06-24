using EPiServer.Core;
using EPiServer.Web.Mvc.Html;
using System.Web.Mvc;

namespace Foundation.Features.Blocks
{
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