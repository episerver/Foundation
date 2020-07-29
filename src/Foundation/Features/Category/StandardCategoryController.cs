using EPiServer;
using EPiServer.Core;
using EPiServer.Core.Html;
using EPiServer.Web.Mvc;
using Foundation.Features.Search;
using Foundation.Features.Shared;
using Geta.EpiCategories;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Foundation.Features.Category
{
    public class StandardCategoryController : ContentController<StandardCategory>
    {
        private readonly ISearchService _searchService;
        private readonly IContentLoader _contentLoader;

        public StandardCategoryController(ISearchService searchService, IContentLoader contentLoader)
        {
            _searchService = searchService;
            _contentLoader = contentLoader;
        }

        public ActionResult Index(StandardCategory currentContent, Pagination pagination)
        {
            var categories = new List<ContentReference> { currentContent.ContentLink };
            pagination.Categories = categories;
            var model = new CategorySearchViewModel(currentContent)
            {
                SearchResults = _searchService.SearchByCategory(pagination)
            };
            return View(model);
        }

        public ActionResult GetListPages(StandardCategory currentContent, Pagination pagination)
        {
            var categories = new List<ContentReference> { currentContent.ContentLink };
            pagination.Categories = categories;
            var model = new CategorySearchViewModel(currentContent)
            {
                SearchResults = _searchService.SearchByCategory(pagination)
            };
            return PartialView("_PageListing", model);
        }

        public ActionResult Preview(FoundationPageData pageData)
        {
            var model = new CategoryFoundationPageViewModel(pageData)
            {
                PreviewText = GetPreviewText(pageData),
                Categories = pageData.Categories.Select(x => _contentLoader.Get<CategoryData>(x) as StandardCategory)
            };
            return PartialView("_Preview", model);
        }

        private string GetPreviewText(FoundationPageData page)
        {
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

            return TextIndexer.StripHtml(previewText, 200);
        }
    }
}