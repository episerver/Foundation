using Foundation.Cms.Pages;
using System.Collections.Generic;

namespace Foundation.Cms.ViewModels
{
    public class BlogListPageViewModel : ContentViewModel<BlogListPage>
    {
        public BlogListPageViewModel(BlogListPage currentPage) : base(currentPage) { }

        public List<KeyValuePair<string, string>> SubNavigation { get; set; }
    }
}
