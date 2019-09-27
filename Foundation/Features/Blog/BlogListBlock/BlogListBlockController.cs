using EPiServer;
using EPiServer.Core;
using EPiServer.Core.Html;
using EPiServer.DataAbstraction;
using EPiServer.Filters;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Cms.Extensions;
using Foundation.Cms.Pages;
using Foundation.Cms.ViewModels;
using Foundation.Cms.ViewModels.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Foundation.Features.Blog.BlogListBlock
{
    [TemplateDescriptor(Default = true)]
    public class BlogListBlockController : BlockController<Cms.Blocks.BlogListBlock>
    {
        private readonly IContentLoader _contentLoader;
        private readonly BlogTagFactory _blogTagFactory;
        private readonly CategoryRepository _categoryRepository;
        private readonly IContentRepository _contentRepository;
        private readonly IPageRouteHelper _pageRouteHelper;

        public BlogListBlockController(IContentLoader contentLoader,
            CategoryRepository categoryRepository,
            BlogTagFactory blogTagFactory,
            IContentRepository contentRepository,
            IPageRouteHelper pageRouteHelper)
        {
            _contentLoader = contentLoader;
            _categoryRepository = categoryRepository;
            _blogTagFactory = blogTagFactory;
            _contentRepository = contentRepository;
            _pageRouteHelper = pageRouteHelper;
        }

        public int PreviewTextLength { get; set; }

        public override ActionResult Index(Cms.Blocks.BlogListBlock currentContent)
        {
            var currentPage = _pageRouteHelper.Page ?? _contentLoader.Get<PageData>(ContentReference.StartPage);
            var pageId = currentPage.ContentLink.ID;
            var pagingInfo = new PagingInfo
            {
                PageId = pageId
            };

            return GetItemList(pagingInfo);
        }

        public ActionResult GetItemList(PagingInfo pagingInfo)
        {
            var currentPage = _contentRepository.Get<PageData>(new PageReference(pagingInfo.PageId)) as Cms.Pages.BlogListPage;

            if (currentPage == null)
            {
                return new EmptyResult();
            }

            var currentBlock = currentPage.BlogList;
            var categoryQuery = Request.QueryString["category"] ?? "";
            var category = _categoryRepository.Get(categoryQuery);
            var pageSize = pagingInfo.PageSize;

            // TODO: Need a better solution to get data by page
            var blogs = FindPages(currentBlock, category, currentPage).ToList();

            blogs = Sort(blogs, currentBlock.SortOrder).ToList();
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

            var model = new BlogListBlockViewModel(currentBlock)
            {
                Blogs = blogs,
                Heading = category != null ? "Blog tags for post: " + category.Name : string.Empty,
                PagingInfo = pagingInfo
            };

            return PartialView("~/Features/Blog/BlogListBlock/Index.cshtml", model);
        }


        public ActionResult Preview(PageData currentPage, BlogListBlockViewModel blogModel)
        {
            var pd = (BlogItemPage)currentPage;
            PreviewTextLength = 200;

            var model = new BlogItemPageModel(pd)
            {
                Tags = GetTags(pd),
                PreviewText = GetPreviewText(pd),
                ShowIntroduction = blogModel.ShowIntroduction,
                ShowPublishDate = blogModel.ShowPublishDate,
                StartPublish = currentPage.StartPublish ?? DateTime.UtcNow
            };

            return PartialView("Preview", model);
        }

        public IEnumerable<BlogItemPageModel.TagItem> GetTags(BlogItemPage currentPage)
        {
            return currentPage.Category.Select(item => _categoryRepository.Get(item)).
                Select(cat => new BlogItemPageModel.TagItem()
                {
                    Title = cat.Name,
                    Url = _blogTagFactory.GetTagUrl(currentPage, cat),
                    DisplayName = cat.Description,
                }).ToList();
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

        private IEnumerable<PageData> FindPages(Cms.Blocks.BlogListBlock currentBlock, Category category, PageData currentPage)
        {
            var listRoot = currentBlock.Root ?? currentPage.ContentLink;

            IEnumerable<PageData> pages;

            if (currentBlock.Recursive)
            {
                pages = currentBlock.PageTypeFilter != null ? listRoot.FindPagesByPageType(true, currentBlock.PageTypeFilter.ID) : listRoot.GetAllRecursively<PageData>();
            }
            else
            {
                if (currentBlock.PageTypeFilter != null)
                {
                    pages = _contentLoader.GetChildren<PageData>(listRoot)
                        .Where(p => p.ContentTypeID == currentBlock.PageTypeFilter.ID);
                }
                else
                {
                    pages = _contentLoader.GetChildren<PageData>(listRoot);
                }
            }

            if (currentBlock.CategoryFilter != null && currentBlock.CategoryFilter.Any())
            {
                pages = pages.Where(x => x.Category.Intersect(currentBlock.CategoryFilter).Any());
            }
            else if (category != null)
            {
                var catlist = new CategoryList
                {
                    category.ID
                };

                pages = pages.Where(x => x.Category.Intersect(catlist).Any());
            }
            pages = pages.Where(p => p.PageTypeName == typeof(BlogItemPage).GetPageType().Name).ToList();
            return pages;
        }

        private List<PageData> Sort(IEnumerable<PageData> pages, FilterSortOrder sortOrder)
        {
            var asCollection = new PageDataCollection(pages);
            var sortFilter = new FilterSort(sortOrder);
            sortFilter.Sort(asCollection);
            return asCollection.ToList();
        }


    }
}
