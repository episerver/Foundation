using EPiServer;
using EPiServer.Cms.Shell;
using EPiServer.Core;
using EPiServer.Core.Html;
using EPiServer.Filters;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Cms.Categories;
using Foundation.Cms.Extensions;
using Foundation.Cms.Pages;
using Foundation.Cms.Personalization;
using Foundation.Cms.ViewModels;
using Geta.EpiCategories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Blog.BlogListPage
{
    public class BlogListPageController : PageController<Cms.Pages.BlogListPage>
    {
        private readonly IContentLoader _contentLoader;
        private readonly UrlResolver _urlResolver;
        private readonly ICmsTrackingService _trackingService;
        private readonly IPageRouteHelper _pageRouteHelper;
        private readonly BlogTagFactory _blogTagFactory;

        public BlogListPageController(IContentLoader contentLoader,
            UrlResolver urlResolver,
            ICmsTrackingService trackingService,
            BlogTagFactory blogTagFactory,
            IPageRouteHelper pageRouteHelper)
        {
            _contentLoader = contentLoader;
            _urlResolver = urlResolver;
            _trackingService = trackingService;
            _blogTagFactory = blogTagFactory;
            _pageRouteHelper = pageRouteHelper;
        }

        public async Task<ActionResult> Index(Cms.Pages.BlogListPage currentPage)
        {
            await _trackingService.PageViewed(HttpContext, currentPage);
            var model = new BlogListPageViewModel(currentPage);
            model.SubNavigation = GetSubNavigation(currentPage);

            var pageId = currentPage.ContentLink.ID;
            var pagingInfo = new PagingInfo
            {
                PageId = pageId
            };

            var viewModel = GetViewModel(currentPage, pagingInfo);
            model.Blogs = viewModel.Blogs;
            model.PagingInfo = pagingInfo;
            return View(model);
        }

        private List<KeyValuePair<string, string>> GetSubNavigation(Cms.Pages.BlogListPage currentPage)
        {
            var subNavigation = new List<KeyValuePair<string, string>>();
            var childrenPages = _contentLoader.GetChildren<PageData>(currentPage.ContentLink).Select(x => x as Cms.Pages.BlogListPage).Where(x => x != null);
            var siblingPages = _contentLoader.GetChildren<PageData>(currentPage.ParentLink).Select(x => x as Cms.Pages.BlogListPage).Where(x => x != null);

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
            var currentPage = _contentLoader.Get<PageData>(new PageReference(pagingInfo.PageId)) as Cms.Pages.BlogListPage;

            if (currentPage == null)
            {
                return new EmptyResult();
            }

            var model = GetViewModel(currentPage, pagingInfo);

            return PartialView("~/Features/Blog/BlogListPage/_BlogList.cshtml", model);
        }

        public BlogListPageViewModel GetViewModel(Cms.Pages.BlogListPage currentPage, PagingInfo pagingInfo)
        {

            var categoryQuery = Request.QueryString["category"] ?? string.Empty;
            IContent category = null;
            if (categoryQuery != string.Empty)
            {
                if (int.TryParse(categoryQuery, out var categoryContentId))
                {
                    var content = _contentLoader.Get<StandardCategory>(new ContentReference(categoryContentId));
                    if (content != null)
                    {
                        category = content;
                    }
                }
            }
            var pageSize = pagingInfo.PageSize;

            // TODO: Need a better solution to get data by page
            var blogs = FindPages(currentPage, category).ToList();

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
                Blogs = blogs,
                Heading = category != null ? "Blog tags for post: " + category.Name : string.Empty,
                PagingInfo = pagingInfo
            };

            return model;
        }

        public ActionResult Preview(PageData currentPage, BlogListPageViewModel blogModel)
        {
            var pd = (BlogItemPage)currentPage;
            PreviewTextLength = 200;

            var model = new BlogItemPageModel(pd)
            {
                Tags = GetTags(pd),
                PreviewText = GetPreviewText(pd),
                ShowIntroduction = blogModel.ShowIntroduction,
                ShowPublishDate = blogModel.ShowPublishDate,
                Template = blogModel.CurrentContent.Template,
                PreviewOption = blogModel.CurrentContent.PreviewOption,
                StartPublish = currentPage.StartPublish ?? DateTime.UtcNow
            };

            return PartialView("Preview", model);
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

        protected string GetPreviewText(BlogItemPage page)
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

            return TextIndexer.StripHtml(previewText, PreviewTextLength);
        }

        private IEnumerable<PageData> FindPages(Cms.Pages.BlogListPage currentPage, IContent category)
        {
            var listRoot = currentPage.Root ?? currentPage.ContentLink;
            var blogListItemPageType = typeof(BlogItemPage).GetPageType();
            IEnumerable<PageData> pages;

            pages = currentPage.IncludeAllLevels ? pages = listRoot.FindPagesByPageType(true, blogListItemPageType.ID) : pages = _contentLoader.GetChildren<PageData>(listRoot);

            if (category != null)
            {
                pages = pages.Where(x =>
                {
                    var contentReferences = ((ICategorizableContent)x).Categories;
                    return contentReferences != null && contentReferences
                               .Intersect(new List<ContentReference>() { category.ContentLink }).Any();
                });
            }
            else if (currentPage.CategoryListFilter != null && currentPage.CategoryListFilter.Any())
            {
                pages = pages.Where(x =>
                {
                    var contentReferences = ((ICategorizableContent)x).Categories;
                    return contentReferences != null &&
                           contentReferences.Intersect(currentPage.CategoryListFilter).Any();
                });
            }

            return pages;
        }

        private List<PageData> Sort(IEnumerable<PageData> pages, FilterSortOrder sortOrder)
        {
            var asCollection = new PageDataCollection(pages);
            var sortFilter = new FilterSort(sortOrder);
            sortFilter.Sort(asCollection);
            return asCollection.ToList();
        }
        #endregion
    }
}
