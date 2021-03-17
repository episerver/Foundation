using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using System;

namespace Foundation.Cms.Extensions
{
    public static class PageTypeExtensions
    {
        private static readonly Lazy<IContentTypeRepository<PageType>> PageTypeRepository =
            new Lazy<IContentTypeRepository<PageType>>(() =>
                ServiceLocator.Current.GetInstance<IContentTypeRepository<PageType>>());

        public static PageType GetPageType(this Type pageType) => PageTypeRepository.Value.Load(pageType);
    }
}