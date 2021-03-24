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

        public static string GetDefaultAsset<T>(this IAssetContainer assetContainer)
            where T : IContentMedia
        {
            var url = AssetUrlResolver.Service.GetAssetUrl<T>(assetContainer);
            if (Uri.TryCreate(url, UriKind.Absolute, out var uri))
            {
                return uri.PathAndQuery;
            }

            return url;
        }

        public static IList<string> GetAssets<T>(this IAssetContainer assetContainer,
            IContentLoader contentLoader, UrlResolver urlResolver)
            where T : IContentMedia
        {
            var assets = new List<string>();
            if (assetContainer.CommerceMediaCollection != null)
            {
                assets.AddRange(assetContainer.CommerceMediaCollection
                    .Where(x => ValidateCorrectType<T>(x.AssetLink, contentLoader))
                    .Select(media => urlResolver.GetUrl(media.AssetLink, null, new VirtualPathArguments() { ContextMode = ContextMode.Default })));
            }

            if (!assets.Any())
            {
                assets.Add(string.Empty);
            }

            return assets;
        }

        public static IList<KeyValuePair<string, string>> GetAssetsWithType(this IAssetContainer assetContainer,
            IContentLoader contentLoader, UrlResolver urlResolver)
        {
            var assets = new List<KeyValuePair<string, string>>();
            if (assetContainer.CommerceMediaCollection != null)
            {
                assets.AddRange(
                    assetContainer.CommerceMediaCollection
                    .Select(media =>
                    {
                        if (contentLoader.TryGet<IContentMedia>(media.AssetLink, out var contentMedia))
                        {
                            var type = "Image";
                            var url = urlResolver.GetUrl(media.AssetLink, null, new VirtualPathArguments() { ContextMode = ContextMode.Default });
                            if (contentMedia is IContentVideo)
                            {
                                type = "Video";
                            }

                            return new KeyValuePair<string, string>(type, url);
                        }

                        return new KeyValuePair<string, string>(string.Empty, string.Empty);
                    })
                    .Where(x => x.Key != string.Empty)
                );
            }

            return assets;
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

        private static bool ValidateCorrectType<T>(ContentReference contentLink,
            IContentLoader contentLoader)
            where T : IContentMedia
        {
            if (typeof(T) == typeof(IContentMedia))
            {
                return true;
            }

            if (ContentReference.IsNullOrEmpty(contentLink))
            {
                return false;
            }

            return contentLoader.TryGet(contentLink, out T _);
        }
    }
}