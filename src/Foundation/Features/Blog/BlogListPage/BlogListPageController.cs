using EPiServer.Cms.Shell;
// CMS 13 removed: EPiServer.Core.Html.TextIndexer removed.
// using EPiServer.Core.Html;
using EPiServer.Filters;
using Foundation.Features.Blog.BlogItemPage;
using Foundation.Infrastructure.Cms;
using System.Text;
using System.Text.RegularExpressions;

namespace Foundation.Features.Blog.BlogListPage
{
    public class BlogListPageController : PageController<BlogListPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;
        private readonly BlogTagFactory _blogTagFactory;

        public BlogListPageController(IContentLoader contentLoader,
            UrlResolver urlResolver,
            BlogTagFactory blogTagFactory)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
            _blogTagFactory = blogTagFactory;
        }
        public ActionResult Index(BlogListPage currentPage)
        {
            var model = new BlogListPageViewModel(currentPage)
            {
                SubNavigation = GetSubNavigation(currentPage)
            };

            var pageId = currentPage.ContentLink.ID;
            var pagingInfo = new PagingInfo
            {
                PageId = pageId
            };

            if (currentPage.Template == TemplateSelections.Card || currentPage.Template == TemplateSelections.Insight)
            {
                pagingInfo.PageSize = 6;
            }

            var viewModel = GetViewModel(currentPage, pagingInfo);
            model.Blogs = viewModel.Blogs;
            model.PagingInfo = pagingInfo;

            return View(model);
        }

        private List<KeyValuePair<string, string>> GetSubNavigation(BlogListPage currentPage)
        {
            var subNavigation = new List<KeyValuePair<string, string>>();
            var childrenPages = _contentLoader.GetChildren<PageData>(currentPage.ContentLink).Select(x => x as BlogListPage).Where(x => x != null);
            var siblingPages = _contentLoader.GetChildren<PageData>(currentPage.ParentLink).Select(x => x as BlogListPage).Where(x => x != null);

            if (siblingPages != null && siblingPages.Count() > 0)
            {
                subNavigation.AddRange(siblingPages.Select(x => new KeyValuePair<string, string>(x.MetaTitle, x.PublicUrl(_urlResolver))));
            }

            // when current page is blog start page
            if (childrenPages != null && childrenPages.Count() > 0)
            {
                subNavigation.AddRange(childrenPages.Select(x => new KeyValuePair<string, string>(x.MetaTitle, x.PublicUrl(_urlResolver))));
            }

            return subNavigation;
        }

        #region BlogListBlock
        public int PreviewTextLength { get; set; }

        public ActionResult GetItemList(PagingInfo pagingInfo)
        {
            var currentPage = _contentLoader.Get<PageData>(new PageReference(pagingInfo.PageId)) as BlogListPage;

            if (currentPage == null)
            {
                return new EmptyResult();
            }

            var model = GetViewModel(currentPage, pagingInfo);

            return PartialView("~/Features/Blog/BlogListPage/Views/_BlogList.cshtml", model);
        }

        public BlogListPageViewModel GetViewModel(BlogListPage currentPage, PagingInfo pagingInfo)
        {
            var pageSize = pagingInfo.PageSize;

            // TODO: Need a better solution to get data by page
            var blogs = FindPages(currentPage).ToList();

            blogs = Sort(blogs, currentPage.SortOrder).ToList();
            pagingInfo.TotalRecord = blogs.Count;

            if (pageSize > 0)
            {
                if (pagingInfo.PageCount < pagingInfo.PageNumber)
                {
                    pagingInfo.PageNumber = pagingInfo.PageCount;
                }
                var skip = (pagingInfo.PageNumber - 1) * pageSize;
                blogs = blogs.Skip(skip).Take(pageSize).ToList();
            }

            var model = new BlogListPageViewModel(currentPage)
            {
                Heading = string.Empty,
                PagingInfo = pagingInfo
            };
            model.Blogs = blogs.Select(x => GetBlogItemPageViewModel(x, model));
            return model;
        }

        private BlogItemPageViewModel GetBlogItemPageViewModel(PageData currentPage, BlogListPageViewModel blogModel)
        {
            var pd = (BlogItemPage.BlogItemPage)currentPage;
            PreviewTextLength = 200;

            var model = new BlogItemPageViewModel(pd)
            {
                Tags = GetTags(pd),
                PreviewText = GetPreviewText(pd),
                ShowIntroduction = blogModel.ShowIntroduction,
                ShowPublishDate = blogModel.ShowPublishDate,
                Template = blogModel.CurrentContent.Template,
                PreviewOption = blogModel.CurrentContent.PreviewOption,
                StartPublish = currentPage.StartPublish ?? DateTime.UtcNow
            };

            return model;
        }

        private IEnumerable<BlogItemPageViewModel.TagItem> GetTags(BlogItemPage.BlogItemPage currentPage)
        {
            // Category-based tags removed: Geta.Optimizely.Categories has no CMS 13 version.
            return new List<BlogItemPageViewModel.TagItem>();
        }

        private string GetPreviewText(BlogItemPage.BlogItemPage page)
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

            //If the MainBody contains DynamicContents, replace those with an empty string
            var regexPattern = new StringBuilder(@"<span[\s\W\w]*?classid=""");
            regexPattern.Append(@"""[\s\W\w]*?</span>");
            previewText = Regex.Replace(previewText, regexPattern.ToString(), string.Empty, RegexOptions.IgnoreCase | RegexOptions.Multiline);

            // CMS 13 removed: EPiServer.Core.Html.TextIndexer removed. Strip HTML manually.
            return StripHtml(previewText, PreviewTextLength);
        }

        private IEnumerable<PageData> FindPages(BlogListPage currentPage)
        {
            var listRoot = currentPage.Root ?? currentPage.ContentLink;
            var blogListItemPageType = typeof(BlogItemPage.BlogItemPage).GetPageType();
            IEnumerable<PageData> pages;

            pages = currentPage.IncludeAllLevels ? listRoot.FindPagesByPageType(true, blogListItemPageType.ID) : _contentLoader.GetChildren<BlogItemPage.BlogItemPage>(listRoot);

            // Category filtering removed: Geta.Optimizely.Categories has no CMS 13 version.
            // CategoryListFilter property is preserved on BlogListPage for future re-implementation.

            return pages;
        }

        private List<PageData> Sort(IEnumerable<PageData> pages, FilterSortOrder sortOrder)
        {
            var asCollection = new PageDataCollection(pages);
            var sortFilter = new FilterSort(sortOrder);
            sortFilter.Sort(asCollection);
            return asCollection.ToList();
        }
        // CMS 13 removed: TextIndexer.StripHtml removed. Replacement using Regex.
        private static string StripHtml(string html, int maxLength)
        {
            if (string.IsNullOrEmpty(html)) return string.Empty;
            var stripped = System.Text.RegularExpressions.Regex.Replace(html, "<[^>]+>", string.Empty);
            stripped = System.Net.WebUtility.HtmlDecode(stripped);
            return maxLength > 0 && stripped.Length > maxLength ? stripped.Substring(0, maxLength) : stripped;
        }

        #endregion
    }
}
