using EPiServer;
using EPiServer.Core.Html;
using Foundation.Features.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using System.Text.RegularExpressions;

namespace Foundation.Features.Category
{
    public class StandardCategoryComponent : ViewComponent
    {
        private readonly IContentLoader _contentLoader;

        public StandardCategoryComponent(
            IContentLoader contentLoader)
        {
            _contentLoader = contentLoader;
        }

        public IViewComponentResult Invoke(FoundationPageData pageData)
        {
            var model = new CategoryFoundationPageViewModel(pageData)
            {
                PreviewText = GetPreviewText(pageData),
                //Categories = pageData.Categories.Select(x => _contentLoader.Get<CategoryData>(x) as StandardCategory)
            };
            return View("_Preview", model);
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
