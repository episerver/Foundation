﻿using EPiServer.Core;
using Foundation.Cms.Pages;
using System.Collections.Generic;

namespace Foundation.Cms.ViewModels
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
        public IEnumerable<PageData> Blogs { get; set; }
        public bool ShowIntroduction { get; set; }
        public bool ShowPublishDate { get; set; }
        public PagingInfo PagingInfo { get; set; }
    }
}
