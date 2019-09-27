using EPiServer.Core;
using Foundation.Cms.Blocks;
using System.Collections.Generic;

namespace Foundation.Cms.ViewModels.Header
{
    public class MenuItemViewModel
    {
        public string Name { get; set; }
        public string Uri { get; set; }
        public string ImageUrl { get; set; }
        public XhtmlString TeaserText { get; set; }
        public string ButtonText { get; set; }
        public string ButtonLink { get; set; }
        public List<GroupLinkCollection> ChildLinks { get; set; }
    }
}
