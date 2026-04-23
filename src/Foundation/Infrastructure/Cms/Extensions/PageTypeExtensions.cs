namespace Foundation.Infrastructure.Cms.Extensions
{
    public static class PageTypeExtensions
    {
        // CMS 13: IContentTypeRepository is no longer generic.
        private static readonly Lazy<IContentTypeRepository> PageTypeRepository =
            new Lazy<IContentTypeRepository>(() =>
                ServiceLocator.Current.GetInstance<IContentTypeRepository>());

        // CMS 13: IContentTypeRepository.Load() returns ContentType, not PageType. Explicit cast required.
        public static PageType GetPageType(this Type pageType) => (PageType)PageTypeRepository.Value.Load(pageType);
    }
}