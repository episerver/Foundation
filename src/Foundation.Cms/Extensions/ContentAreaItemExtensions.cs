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
        private static readonly Lazy<IContentLoader> ContentLoader =
            new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        public static IList<T> GetContentItems<T>(this IEnumerable<ContentAreaItem> contentAreaItems) where T : IContentData
        {
            if (contentAreaItems == null || !contentAreaItems.Any())
            {
                return null;
            }

            return ContentLoader.Value
                .GetItems(contentAreaItems.Select(_ => _.ContentLink), new LoaderOptions { LanguageLoaderOption.FallbackWithMaster() })
                .OfType<T>()
                .ToList();
        }

        public static T GetBlock<T>(this ContentAreaItem contentAreaItem) where T : BlockData
        {
            if (contentAreaItem == null)
            {
                return null;
            }

            return ContentLoader.Value.Get<IContentData>(contentAreaItem.ContentLink) as T;
        }

        //public static IBlockViewModel<T> GetBlockViewModel<T>(this ContentAreaItem contentAreaItem) where T : BlockData
        //{
        //    var block = GetBlock<T>(contentAreaItem);
        //    return block != null ? CreateModel(block) : null;
        //}

        //private static IBlockViewModel<T> CreateModel<T>(T currentBlock) where T : BlockData
        //{
        //    var type = typeof(BlockViewModel<>).MakeGenericType(currentBlock.GetOriginalType());
        //    return Activator.CreateInstance(type, currentBlock) as IBlockViewModel<T>;
        //}
    }
}