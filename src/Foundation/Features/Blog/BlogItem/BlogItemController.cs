using EPiServer;
using EPiServer.Cms.Shell;
using EPiServer.Core.Html;
using EPiServer.DataAbstraction;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Cms.Categories;
using Foundation.Cms.Pages;
using Foundation.Cms.Personalization;
using Foundation.Cms.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Blog.BlogItem
{
    public class BlogItemController : PageController<BlogItemPage>
    {
        private readonly BlogTagFactory _blogTagFactory;
        private readonly CategoryRepository _categoryRepository;
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;
        private readonly ICmsTrackingService _trackingService;

        public int PreviewTextLength { get; set; }

        public BlogItemController(BlogTagFactory blogTagFactory,
            CategoryRepository categoryRepository,
            IContentLoader contentLoader,
            UrlResolver urlResolver, ICmsTrackingService trackingService)
        {
            _blogTagFactory = blogTagFactory;
            _categoryRepository = categoryRepository;
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
            _trackingService = trackingService;
        }

        public async Task<ActionResult> Index(BlogItemPage currentPage)
        {
            await _trackingService.PageViewed(HttpContext, currentPage);
            PreviewTextLength = 200;

            var model = new BlogItemPageModel(currentPage)
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

        public IEnumerable<BlogItemPageModel.TagItem> GetTags(BlogItemPage currentPage)
        {
            if (currentPage.Categories != null)
            {
                var allCategories = _contentLoader.GetItems(currentPage.Categories, CultureInfo.CurrentUICulture);
                return allCategories.
                    Select(cat => new BlogItemPageModel.TagItem()
                    {
                        Title = cat.Name,
                        Url = _blogTagFactory.GetTagUrl(currentPage, cat.ContentLink),
                        DisplayName = (cat as StandardCategory)?.Description,
                    }).ToList();
            }
            return new List<BlogItemPageModel.TagItem>();
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
                .Select(x => x as Cms.Pages.BlogListPage)
                .Where(x => x != null);
            breadCrumb = ancestors.Reverse().Select(x => new KeyValuePair<string, string>(x.MetaTitle, x.PublicUrl(_urlResolver))).ToList();

            return breadCrumb;
        }
    }
}