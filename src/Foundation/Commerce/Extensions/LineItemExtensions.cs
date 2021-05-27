using EPiServer;
using EPiServer.Commerce.Catalog;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Order;
using EPiServer.Commerce.Reporting.Order.ReportingModels;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using Mediachase.Commerce.Catalog;
using System;
using System.Web;

namespace Foundation.Commerce.Extensions
{
    public static class LineItemExtensions
    {
        private static readonly Lazy<ReferenceConverter> _referenceConverter =
            new Lazy<ReferenceConverter>(() => ServiceLocator.Current.GetInstance<ReferenceConverter>());

        private static readonly Lazy<IContentLoader> _contentLoader =
            new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        private static readonly Lazy<ThumbnailUrlResolver> _thumbnailUrlResolver =
            new Lazy<ThumbnailUrlResolver>(() => ServiceLocator.Current.GetInstance<ThumbnailUrlResolver>());

        public static string GetUrl(this ILineItem lineItem) => lineItem.GetEntryContent()?.GetUrl();

        public static string GetFullUrl(this ILineItem lineItem)
        {
            var rightUrl = lineItem.GetUrl();
            var baseUrl = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
            return new Uri(new Uri(baseUrl), rightUrl).ToString();
        }

        public static string GetThumbnailUrl(this ILineItem lineItem) => GetThumbnailUrl(lineItem.Code);

        private static string GetThumbnailUrl(string code)
        {
            var content = GetEntryContent<EntryContentBase>(code);
            if (content == null)
            {
                return string.Empty;
            }

            return _thumbnailUrlResolver.Value.GetThumbnailUrl(content, "thumbnail");
        }

        public static T GetEntryContent<T>(string code) where T : EntryContentBase
        {
            var entryContentLink = _referenceConverter.Value.GetContentLink(code);
            if (ContentReference.IsNullOrEmpty(entryContentLink))
            {
                return null;
            }

            return _contentLoader.Value.Get<T>(entryContentLink);
        }

        public static EntryContentBase GetEntryContentBase(this ILineItem lineItem) => GetEntryContent<EntryContentBase>(lineItem.Code);

        public static EntryContentBase GetEntryContentBase(this LineItemReportingModel lineItem) => GetEntryContent<EntryContentBase>(lineItem.LineItemCode);

        public static T GetEntryContent<T>(this ILineItem lineItem) where T : EntryContentBase => GetEntryContent<T>(lineItem.Code);

        public static ContentReference GetContentReference(this LinkItem linkItem)
        {
            var guid = PermanentLinkUtility.GetGuid(new UrlBuilder(linkItem.GetMappedHref()), out _);
            return PermanentLinkUtility.FindContentReference(guid);
        }
    }
}