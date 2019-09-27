using EPiServer.Core;
using Foundation.Cms.Blocks;
using System.Collections.Generic;

namespace Foundation.Cms.ViewModels.Blocks
{
    public class RssReaderBlockViewModel
    {
        public RssReaderBlock CurrentBlock { get; set; }

        public XhtmlString DescriptiveText { get; set; }
        public bool HasHeadingText { get; set; }
        public string Heading { get; set; }
        public List<RssItem> RssList { get; set; }

        public class RssItem
        {
            public string Title { get; set; }
            public string Url { get; set; }
            public string PublishDate { get; set; }
        }
    }
}
