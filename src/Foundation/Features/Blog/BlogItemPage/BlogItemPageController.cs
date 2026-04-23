using EPiServer.Cms.Shell;
// CMS 13 removed: EPiServer.Core.Html.TextIndexer removed.
// using EPiServer.Core.Html;
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
            // Category filtering removed: Geta.Optimizely.Categories has no CMS 13 version.
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

            // CMS 13 removed: EPiServer.Core.Html.TextIndexer removed. Strip HTML manually.
            return StripHtml(previewText, PreviewTextLength);
        }

        // CMS 13 removed: TextIndexer.StripHtml removed. Replacement using Regex.
        private static string StripHtml(string html, int maxLength)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;
            var stripped = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", string.Empty);
            stripped = System.Net.WebUtility.HtmlDecode(stripped);
            return maxLength > 0 && stripped.Length > maxLength ? stripped.Substring(0, maxLength) : stripped;
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