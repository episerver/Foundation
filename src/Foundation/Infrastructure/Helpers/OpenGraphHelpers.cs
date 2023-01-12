using Boxed.AspNetCore.TagHelpers.OpenGraph;
using Foundation.Features.Blog.BlogItemPage;
using Foundation.Features.Home;
using Foundation.Features.Locations.LocationItemPage;
using Foundation.Features.Locations.TagPage;
using Foundation.Features.StandardPage;
using Foundation.Infrastructure.Cms.Settings;
using Foundation.Infrastructure.OpenGraph;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Foundation.Infrastructure.Helpers
{
    public static class OpenGraphHelpers
    {
        private static readonly Lazy<IContentLoader> _contentLoader = new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());
        private static readonly Lazy<IContentTypeRepository> _contentTypeRepository = new Lazy<IContentTypeRepository>(() => ServiceLocator.Current.GetInstance<IContentTypeRepository>());
        private static readonly Lazy<ISettingsService> _settingsService = new Lazy<ISettingsService>(() => ServiceLocator.Current.GetInstance<ISettingsService>());
        private static readonly Lazy<IContentLanguageAccessor> _cultureAccessor = new Lazy<IContentLanguageAccessor>(() => ServiceLocator.Current.GetInstance<IContentLanguageAccessor>());

        public static LayoutSettings GetLayoutSettings(this IHtmlHelper helper) => _settingsService.Value.GetSiteSettings<LayoutSettings>();
        public static IHtmlContent RenderOpenGraphMetaData(this IHtmlHelper helper, IContentViewModel<IContent> contentViewModel)
        {
            var metaTitle = (contentViewModel.CurrentContent as FoundationPageData)?.MetaTitle ?? contentViewModel.CurrentContent.Name;
            var defaultLocale = _cultureAccessor.Value.Language;
            IEnumerable<string> alternateLocales = null;
            string contentType = null;
            string imageUrl = null;

            if (contentViewModel.CurrentContent is FoundationPageData && ((FoundationPageData)contentViewModel.CurrentContent).PageImage != null)
            {
                imageUrl = GetUrl(((FoundationPageData)contentViewModel.CurrentContent).PageImage);
            }
            else
            {
                imageUrl = GetDefaultImageUrl();
            }

            if (contentViewModel.CurrentContent is FoundationPageData pageData)
            {
                alternateLocales = pageData.ExistingLanguages.Where(culture => culture != defaultLocale)
                            .Select(culture => culture.TextInfo.CultureName.Replace('-', '_'));
            }

            if (contentViewModel.CurrentContent is FoundationPageData)
            {
                if (((FoundationPageData)contentViewModel.CurrentContent).MetaContentType != null)
                {
                    contentType = ((FoundationPageData)contentViewModel.CurrentContent).MetaContentType;
                }
                else
                {
                    var pageType = _contentTypeRepository.Value.Load(contentViewModel.CurrentContent.GetOriginalType());
                    contentType = pageType.DisplayName;
                }
            }

            switch (contentViewModel.CurrentContent)
            {
                case HomePage homePage:
                    var openGraphHomePage = new OpenGraphHomePage(metaTitle, new OpenGraphImage(new Uri(imageUrl)), GetUrl(homePage.ContentLink))
                    {
                        Description = homePage.PageDescription,
                        Locale = defaultLocale.Name.Replace('-', '_'),
                        AlternateLocales = alternateLocales,
                        ContentType = contentType,
                        //Category = GetCategoryNames(homePage.Categories),
                        ModifiedTime = homePage.Changed,
                        PublishedTime = homePage.StartPublish ?? null,
                        ExpirationTime = homePage.StopPublish ?? null
                    };

                    return helper.OpenGraph(openGraphHomePage);

                case LocationItemPage locationItemPage:
                    var openGraphLocationItemPage = new OpenGraphLocationItemPage(metaTitle, new OpenGraphImage(new Uri(imageUrl)), GetUrl(contentViewModel.CurrentContent.ContentLink))
                    {
                        Description = locationItemPage.PageDescription,
                        Locale = defaultLocale.Name.Replace('-', '_'),
                        AlternateLocales = alternateLocales,
                        ContentType = contentType,
                        ModifiedTime = locationItemPage.Changed,
                        PublishedTime = locationItemPage.StartPublish ?? null,
                        ExpirationTime = locationItemPage.StopPublish ?? null
                    };

                    var categories = new List<string>();

                    if (locationItemPage.Continent != null)
                    {
                        categories.Add(locationItemPage.Continent);
                    }

                    if (locationItemPage.Country != null)
                    {
                        categories.Add(locationItemPage.Country);
                    }

                    //openGraphLocationItemPage.Category = categories;

                    //var tags = new List<string>();
                    //var items = ((LocationItemPage)contentViewModel.CurrentContent).Categories;
                    //if (items != null)
                    //{
                    //    foreach (var item in items)
                    //    {
                    //        tags.Add(_contentLoader.Value.Get<StandardCategory>(item).Name);
                    //    }
                    //}
                    //openGraphLocationItemPage.Tags = tags;

                    return helper.OpenGraph(openGraphLocationItemPage);

                case BlogItemPage _:
                case StandardPage _:
                case TagPage _:
                    var openGraphArticle = new OpenGraphFoundationPageData(metaTitle, new OpenGraphImage(new Uri(imageUrl)), GetUrl(contentViewModel.CurrentContent.ContentLink))
                    {
                        Description = ((FoundationPageData)contentViewModel.CurrentContent).PageDescription,
                        Locale = defaultLocale.Name.Replace('-', '_'),
                        AlternateLocales = alternateLocales,
                        ContentType = contentType,
                        ModifiedTime = ((FoundationPageData)contentViewModel.CurrentContent).Changed,
                        PublishedTime = ((FoundationPageData)contentViewModel.CurrentContent).StartPublish ?? null,
                        ExpirationTime = ((FoundationPageData)contentViewModel.CurrentContent).StopPublish ?? null
                    };

                    return helper.OpenGraph(openGraphArticle);

                case FoundationPageData foundationPageData:
                    var openGraphFoundationPage = new OpenGraphFoundationPageData(metaTitle, new OpenGraphImage(new Uri(imageUrl)), GetUrl(foundationPageData.ContentLink))
                    {
                        Description = foundationPageData.PageDescription,
                        Locale = defaultLocale.Name.Replace('-', '_'),
                        AlternateLocales = alternateLocales,
                        Author = foundationPageData.AuthorMetaData,
                        ContentType = contentType,
                        //Category = GetCategoryNames(foundationPageData.Categories),
                        ModifiedTime = foundationPageData.Changed,
                        PublishedTime = foundationPageData.StartPublish ?? null,
                        ExpirationTime = foundationPageData.StopPublish ?? null
                    };

                    return helper.OpenGraph(openGraphFoundationPage);
            }

            return new HtmlString(string.Empty);
        }

        private static string GetDefaultImageUrl()
        {
            var layoutSettings = _settingsService.Value.GetSiteSettings<LayoutSettings>();
            if (layoutSettings?.SiteLogo.IsNullOrEmpty() ?? true)
            {
                return "https://via.placeholder.com/150";
            }
            var startPage = _contentLoader.Value.Get<HomePage>(ContentReference.StartPage);
            var siteUrl = SiteDefinition.Current.SiteUrl;
            var url = new Uri(siteUrl, UrlResolver.Current.GetUrl(layoutSettings.SiteLogo));

            return url.ToString();
        }

        private static string GetUrl(ContentReference content)
        {
            var siteUrl = SiteDefinition.Current.SiteUrl;
            var url = new Uri(siteUrl, UrlResolver.Current.GetUrl(content));

            return url.ToString();
        }

        private static IEnumerable<string> GetCategoryNames(IEnumerable<ContentReference> categories)
        {
            if (categories == null)
            {
                yield break;
            }
            foreach (var category in categories)
            {
                yield return _contentLoader.Value.Get<IContent>(category).Name;
            }
        }
    }
}