using EPiServer.Core;
using Foundation.Cms.Blocks;
using System.Collections.Generic;

namespace Foundation.Cms.ViewModels.Blocks
{
    public class BlogListBlockViewModel : BlockViewModel<BlogListBlock>
    {
        public BlogListBlockViewModel(BlogListBlock block) : base(block)
        {
            Heading = block.Heading;
            ShowIntroduction = block.IncludeIntroduction;
            ShowPublishDate = block.IncludePublishDate;
        }
        public string Heading { get; set; }
        public IEnumerable<PageData> Blogs { get; set; }
        public bool ShowIntroduction { get; set; }
        public bool ShowPublishDate { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}
