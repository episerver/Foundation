using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Framework.Web;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Cms.Extensions
{
    public static class ContentExtensions
    {
        private static readonly CookieService _cookieService = new CookieService();
        private static readonly Injected<IContentLoader> _contentLoader = default(Injected<IContentLoader>);
        private const string Delimiter = "^!!^";

        public static IEnumerable<PageData> GetSiblings(this PageData pageData) => GetSiblings(pageData, _contentLoader.Service);

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
                templateFilter.TemplateTypeCategories = TemplateTypeCategories.Page;
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

            var history = _cookieService.Get("PageBrowseHistory");
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

            _cookieService.Set("PageBrowseHistory", string.Join(Delimiter, values));
        }

        public static IList<PageData> GetPageBrowseHistory()
        {
            var pageIds = _cookieService.Get("PageBrowseHistory");
            if (string.IsNullOrEmpty(pageIds))
            {
                return new List<PageData>();
            }

            var contentLinks = pageIds.Split(new[]
            {
                Delimiter
            }, StringSplitOptions.RemoveEmptyEntries).Select(x => new ContentReference(x));
            return _contentLoader.Service.GetItems(contentLinks, new LoaderOptions())
                .OfType<PageData>()
                .ToList();
        }
    }
}