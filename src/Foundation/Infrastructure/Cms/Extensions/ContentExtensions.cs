using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Infrastructure.Cms.Extensions
{
    public static class ContentExtensions
    {
        private static readonly Lazy<CookieService> _cookieService = new Lazy<CookieService>(() => ServiceLocator.Current.GetInstance<CookieService>());
        private static readonly Lazy<IContentLoader> _contentLoader = new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());
        private const string Delimiter = "^!!^";

        public static IEnumerable<PageData> GetSiblings(this PageData pageData) => GetSiblings(pageData, _contentLoader.Value);

        public static IEnumerable<PageData> GetSiblings(this PageData pageData, IContentLoader contentLoader)
        {
            var filter = new FilterContentForVisitor();
            return contentLoader.GetChildren<PageData>(pageData.ParentLink).Where(page => !filter.ShouldFilter(page));
        }

        public static IEnumerable<T> FilterForDisplay<T>(this IEnumerable<T> contents, bool requirePageTemplate = false,
            bool requireVisibleInMenu = false)
            where T : IContent
        {
            var accessFilter = new FilterAccess();
            var publishedFilter = new FilterPublished();
            contents = contents.Where(x => !publishedFilter.ShouldFilter(x) && !accessFilter.ShouldFilter(x));
            if (requirePageTemplate)
            {
                var templateFilter = ServiceLocator.Current.GetInstance<FilterTemplate>();
                templateFilter.TemplateTypeCategories = TemplateTypeCategories.Request;
                contents = contents.Where(x => !templateFilter.ShouldFilter(x));
            }

            if (requireVisibleInMenu)
            {
                contents = contents.Where(x => VisibleInMenu(x));
            }

            return contents;
        }

        private static bool VisibleInMenu(IContent content)
        {
            var page = content as PageData;
            return page == null || page.VisibleInMenu;
        }

        public static void AddPageBrowseHistory(this PageData page)
        {

            var history = _cookieService.Value.Get("PageBrowseHistory");
            var values = string.IsNullOrEmpty(history) ? new List<int>() :
                history.Split(new[] { Delimiter }, StringSplitOptions.RemoveEmptyEntries).Select(x => Convert.ToInt32(x)).ToList();

            if (values.Contains(page.ContentLink.ID))
            {
                return;
            }

            if (values.Any())
            {
                if (values.Count == 2)
                {
                    values.RemoveAt(0);
                }
            }

            values.Add(page.ContentLink.ID);

            _cookieService.Value.Set("PageBrowseHistory", string.Join(Delimiter, values));
        }

        public static IList<PageData> GetPageBrowseHistory()
        {
            var pageIds = _cookieService.Value.Get("PageBrowseHistory");
            if (string.IsNullOrEmpty(pageIds))
            {
                return new List<PageData>();
            }

            var contentLinks = pageIds.Split(new[]
            {
                Delimiter
            }, StringSplitOptions.RemoveEmptyEntries).Select(x => new ContentReference(x));
            return _contentLoader.Value.GetItems(contentLinks, new LoaderOptions())
                .OfType<PageData>()
                .ToList();
        }

        /// <summary>
        /// Helper method to get a URL string for an IContent
        /// </summary>
        /// <param name="content">The routable content item to get the URL for.</param>
        /// <param name="isAbsolute">Whether the full URL including protocol and host should be returned.</param>
        public static string GetUrl<T>(this T content, bool isAbsolute = false) where T : IContent, ILocale, IRoutable
        {
            return content.GetUri(isAbsolute).ToString();
        }

        /// <summary>
        /// Helper method to get a Uri for an IContent
        /// </summary>
        /// <param name="content">The routable content item to get the URL for.</param>
        /// <param name="isAbsolute">Whether the full URL including protocol and host should be returned.</param>
        public static Uri GetUri<T>(this T content, bool isAbsolute = false) where T : IContent, ILocale, IRoutable
        {
            return content.ContentLink.GetUri(content.Language.Name, isAbsolute);
        }
    }
}