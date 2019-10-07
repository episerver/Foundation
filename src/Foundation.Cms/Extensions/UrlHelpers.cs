using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web.Routing;
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Foundation.Cms.Extensions
{
    public static class UrlHelpers
    {
        private static readonly Lazy<IUrlResolver> UrlResolver =
           new Lazy<IUrlResolver>(() => ServiceLocator.Current.GetInstance<IUrlResolver>());

        private static readonly Lazy<IContentLoader> ContentLoader =
            new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        public static RouteValueDictionary ContentRoute(this UrlHelper urlHelper,
            ContentReference contentLink,
            object routeValues = null)
        {
            var first = new RouteValueDictionary(routeValues);

            var values = first.Union(urlHelper.RequestContext.RouteData.Values);

            values[RoutingConstants.ActionKey] = "index";
            values[RoutingConstants.NodeKey] = contentLink;
            return values;
        }

        /// <summary>
        ///     Returns the target URL for a PageReference. Respects the page's shortcut setting
        ///     so if the page is set as a shortcut to another page or an external URL that URL
        ///     will be returned.
        /// </summary>
        public static IHtmlString PageLinkUrl(this UrlHelper urlHelper,
            ContentReference pageLink)
        {
            if (ContentReference.IsNullOrEmpty(pageLink))
                return MvcHtmlString.Empty;

            var page = ContentLoader.Value.Get<PageData>(pageLink);

            return PageLinkUrl(urlHelper, page);
        }

        /// <summary>
        ///     Returns the target URL for a page. Respects the page's shortcut setting
        ///     so if the page is set as a shortcut to another page or an external URL that URL
        ///     will be returned.
        /// </summary>
        public static IHtmlString PageLinkUrl(this UrlHelper urlHelper,
            PageData page)
        {
            switch (page.LinkType)
            {
                case PageShortcutType.Normal:
                case PageShortcutType.FetchData:
                    return new MvcHtmlString(UrlResolver.Value.GetUrl(page.PageLink));

                case PageShortcutType.Shortcut:
                    var shortcutProperty = page.Property["PageShortcutLink"] as PropertyPageReference;
                    if (shortcutProperty != null && !ContentReference.IsNullOrEmpty(shortcutProperty.PageLink))
                        return urlHelper.PageLinkUrl(shortcutProperty.PageLink);
                    break;

                case PageShortcutType.External:
                    return new MvcHtmlString(page.LinkURL);
            }

            return MvcHtmlString.Empty;
        }

        public static IHtmlString GetSegmentedUrl(this UrlHelper urlHelper,
            PageData currentPage,
            params string[] segments)
        {
            var url = urlHelper.PageLinkUrl(currentPage).ToString();

            if (!url.EndsWith("/"))
                url = url + '/';
            url += string.Join("/", segments);
            //TODO: Url-encode segments

            return new HtmlString(url);
        }

        public static IHtmlString ImageExternalUrl(this UrlHelper urlHelper,
            ImageData image)
        {
            return new MvcHtmlString(UrlResolver.Value.GetUrl(image.ContentLink));
        }

        public static IHtmlString ImageExternalUrl(this UrlHelper urlHelper,
            ImageData image,
            string variant) => urlHelper.ImageExternalUrl(image.ContentLink, variant);

        public static IHtmlString ImageExternalUrl(this UrlHelper urlHelper,
            Uri imageUri,
            string variant)
        {
            return new MvcHtmlString(
                string.IsNullOrWhiteSpace(variant) ? imageUri.ToString() : imageUri + "/" + variant);
        }

        public static IHtmlString ImageExternalUrl(this UrlHelper urlHelper,
            ContentReference imageref,
            string variant)
        {
            if (ContentReference.IsNullOrEmpty(imageref))
                return MvcHtmlString.Empty;

            var url = UrlResolver.Value.GetUrl(imageref);
            //Inject variant
            if (!string.IsNullOrEmpty(variant))
                if (url.Contains("?"))
                    url = url.Insert(url.IndexOf('?'), "/" + variant);
                else
                    url = url + "/" + variant;
            return new MvcHtmlString(url);
        }

        public static IHtmlString CampaignUrl(this UrlHelper urlHelper,
            IHtmlString url,
            string campaign)
        {
            var s = url.ToString();
            if (s.Contains("?"))
                return new MvcHtmlString(s + "&utm_campaign=" + HttpContext.Current.Server.UrlEncode(campaign));
            return new MvcHtmlString(s + "?utm_campaign=" + HttpContext.Current.Server.UrlEncode(campaign));
        }

        public static IHtmlString GetFriendlyUrl(this UrlHelper urlHelper, string url)
        {
            return new HtmlString(UrlResolver.Value.GetUrl(url) ?? url);

        }

        private static IHtmlString WriteShortenedUrl(string root, string segment)
        {
            var fullUrlPath = string.Format("{0}{1}/", root, segment.ToLower().Replace(" ", "-"));

            return new MvcHtmlString(fullUrlPath);
        }

        private static RouteValueDictionary Union(this RouteValueDictionary first,
           RouteValueDictionary second)
        {
            var dictionary = new RouteValueDictionary(second);
            foreach (var pair in first)
                if (pair.Value != null)
                    dictionary[pair.Key] = pair.Value;

            return dictionary;
        }
    }
}