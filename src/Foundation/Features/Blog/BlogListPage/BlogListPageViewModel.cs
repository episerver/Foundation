using Foundation.Cms;
using Foundation.Features.Blog.BlogItemPage;
using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.Blog.BlogListPage
{
    public class BlogListPageViewModel : ContentViewModel<BlogListPage>
    {
        public BlogListPageViewModel(BlogListPage currentPage) : base(currentPage)
        {
            Heading = currentPage.Heading;
            ShowIntroduction = currentPage.IncludeTeaserText;
            ShowPublishDate = currentPage.IncludePublishDate;
        }

        public List<KeyValuePair<string, string>> SubNavigation { get; set; }
        public string Heading { get; set; }
        public IEnumerable<BlogItemPageModel> Blogs { get; set; }
        public bool ShowIntroduction { get; set; }
        public bool ShowPublishDate { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
