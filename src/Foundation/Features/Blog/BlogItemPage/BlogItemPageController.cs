using EPiServer;
using EPiServer.Cms.Shell;
using EPiServer.Core.Html;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
//using Foundation.Features.Category;
using Foundation.Features.Shared;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Foundation.Features.Blog.BlogItemPage
{
    public class BlogItemPageController : PageController<BlogItemPage>
    {
        private readonly BlogTagFactory _blogTagFactory;
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;

        public int PreviewTextLength { get; set; }

        public BlogItemPageController(BlogTagFactory blogTagFactory,
            IContentLoader contentLoader,
            UrlResolver urlResolver)
        {
            _blogTagFactory = blogTagFactory;
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
        }

        public ActionResult Index(BlogItemPage currentPage)
        {
            PreviewTextLength = 200;

            var model = new BlogItemPageViewModel(currentPage)
            {
                Category = currentPage.Category,
                Tags = GetTags(currentPage),
                PreviewText = GetPreviewText(currentPage),
                MainBody = currentPage.MainBody,
                StartPublish = currentPage.StartPublish ?? DateTime.UtcNow,
                BreadCrumbs = GetBreadCrumb(currentPage)
            };

            var editHints = ViewData.GetEditHints<ContentViewModel<BlogItemPage>, BlogItemPage>();
            editHints.AddConnection(m => m.CurrentContent.Category, p => p.Category);
            editHints.AddConnection(m => m.CurrentContent.StartPublish, p => p.StartPublish);

            return View(model);
        }

        public IEnumerable<BlogItemPageViewModel.TagItem> GetTags(BlogItemPage currentPage)
        {
            //if (currentPage.Categories != null)
            //{
            //    var allCategories = _contentLoader.GetItems(currentPage.Categories, CultureInfo.CurrentUICulture);
            //    return allCategories.
            //        Select(cat => new BlogItemPageViewModel.TagItem()
            //        {
            //            Title = cat.Name,
            //            Url = _blogTagFactory.GetTagUrl(currentPage, cat.ContentLink),
            //            DisplayName = (cat as StandardCategory)?.Description,
            //        }).ToList();
            //}
            return new List<BlogItemPageViewModel.TagItem>();
        }

        private string GetPreviewText(BlogItemPage page)
        {
            if (PreviewTextLength <= 0)
            {
                return string.Empty;
            }

            var previewText = string.Empty;

            if (page.MainBody != null)
            {
                previewText = page.MainBody.ToHtmlString();
            }

            if (string.IsNullOrEmpty(previewText))
            {
                return string.Empty;
            }

            var regexPattern = new StringBuilder(@"<span[\s\W\w]*?classid=""");
            //regexPattern.Append(DynamicContentFactory.Instance.DynamicContentId.ToString());
            regexPattern.Append(@"""[\s\W\w]*?</span>");
            previewText = Regex.Replace(previewText, regexPattern.ToString(), string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            return TextIndexer.StripHtml(previewText, PreviewTextLength);
        }

        private List<KeyValuePair<string, string>> GetBreadCrumb(BlogItemPage currentPage)
        {
            var breadCrumb = new List<KeyValuePair<string, string>>();
            var ancestors = _contentLoader.GetAncestors(currentPage.ContentLink)
                .Select(x => x as BlogListPage.BlogListPage)
                .Where(x => x != null);
            breadCrumb = ancestors.Reverse().Select(x => new KeyValuePair<string, string>(x.MetaTitle, x.PublicUrl(_urlResolver))).ToList();

            return breadCrumb;
        }
    }
}