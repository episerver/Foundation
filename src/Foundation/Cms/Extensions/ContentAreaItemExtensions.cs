using EPiServer;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Cms.Extensions
{
    public static class ContentAreaItemExtensions
    {
        private static readonly Lazy<IContentLoader> _contentLoader =
            new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        public static IList<T> GetContentItems<T>(this IEnumerable<ContentAreaItem> contentAreaItems) where T : IContentData
        {
            if (contentAreaItems == null || !contentAreaItems.Any())
            {
                return null;
            }

            return _contentLoader.Value
                .GetItems(contentAreaItems.Select(_ => _.ContentLink), new LoaderOptions { LanguageLoaderOption.FallbackWithMaster() })
                .OfType<T>()
                .ToList();
        }
    }
}