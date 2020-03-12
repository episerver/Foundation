using EPiServer;
using EPiServer.Commerce.Catalog;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Extensions
{
    public static class AssetContainerExtensions
    {
        private static readonly Injected<AssetUrlResolver> AssetUrlResolver;

        public static string GetDefaultAsset<TContentMedia>(this IAssetContainer assetContainer)
            where TContentMedia : IContentMedia
        {
            var url = AssetUrlResolver.Service.GetAssetUrl<TContentMedia>(assetContainer);
            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                return uri.PathAndQuery;
            }

            return url;
        }

        public static IList<string> GetAssets<TContentMedia>(this IAssetContainer assetContainer,
            IContentLoader contentLoader, UrlResolver urlResolver)
            where TContentMedia : IContentMedia
        {
            var assets = new List<string>();
            if (assetContainer.CommerceMediaCollection != null)
            {
                assets.AddRange(assetContainer.CommerceMediaCollection
                    .Where(x => ValidateCorrectType<TContentMedia>(x.AssetLink, contentLoader))
                    .Select(media => urlResolver.GetUrl(media.AssetLink, null, new VirtualPathArguments() { ContextMode = ContextMode.Default })));
            }

            if (!assets.Any())
            {
                assets.Add(string.Empty);
            }

            return assets;
        }

        private static bool ValidateCorrectType<TContentMedia>(ContentReference contentLink,
            IContentLoader contentLoader)
            where TContentMedia : IContentMedia
        {
            if (typeof(TContentMedia) == typeof(IContentMedia))
            {
                return true;
            }

            if (ContentReference.IsNullOrEmpty(contentLink))
            {
                return false;
            }

            return contentLoader.TryGet(contentLink, out TContentMedia _);
        }

        public static IList<MediaData> GetAssetsMediaData(this IAssetContainer assetContainer, IContentLoader contentLoader, string groupName = "")
        {
            if (assetContainer.CommerceMediaCollection != null)
            {
                var assets = assetContainer.CommerceMediaCollection
                    .Where(x => string.IsNullOrEmpty(groupName) || x.GroupName == groupName)
                    .Select(x => contentLoader.Get<IContent>(x.AssetLink) as MediaData)
                    .Where(x => x != null)
                    .ToList();

                return assets;
            }

            return new List<MediaData>();
        }
    }
}