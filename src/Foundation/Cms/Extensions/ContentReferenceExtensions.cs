using EPiServer;
using EPiServer.Core;
using EPiServer.Filters;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Foundation.Cms.Extensions
{
    public static class ContentReferenceExtensions
    {
        private static readonly Lazy<IContentLoader> ContentLoader =
            new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        private static readonly Lazy<IContentProviderManager> ProviderManager =
            new Lazy<IContentProviderManager>(() => ServiceLocator.Current.GetInstance<IContentProviderManager>());

        private static readonly Lazy<IPageCriteriaQueryService> PageCriteriaQueryService =
            new Lazy<IPageCriteriaQueryService>(() => ServiceLocator.Current.GetInstance<IPageCriteriaQueryService>());

        private static readonly Lazy<IUrlResolver> UrlResolver =
            new Lazy<IUrlResolver>(() => ServiceLocator.Current.GetInstance<IUrlResolver>());

        private static readonly Lazy<ISiteDefinitionResolver> SiteDefinitionResolver =
            new Lazy<ISiteDefinitionResolver>(() => ServiceLocator.Current.GetInstance<ISiteDefinitionResolver>());

        public static bool IsNullOrEmpty(this ContentReference contentReference) => ContentReference.IsNullOrEmpty(contentReference);

        public static IContent Get<TContent>(this ContentReference contentLink) where TContent : IContent => ContentLoader.Value.Get<TContent>(contentLink);

        public static IEnumerable<T> GetAllRecursively<T>(this ContentReference rootLink) where T : PageData
        {
            foreach (var child in ContentLoader.Value.GetChildren<T>(rootLink))
            {
                yield return child;

                foreach (var descendant in GetAllRecursively<T>(child.ContentLink))
                {
                    yield return descendant;
                }
            }
        }

        public static IEnumerable<PageData> FindPagesByPageType(this ContentReference pageLink, bool recursive, int pageTypeId)
        {
            if (ContentReference.IsNullOrEmpty(pageLink))
            {
                throw new ArgumentNullException(nameof(pageLink), "No page link specified, unable to find pages");
            }

            return recursive
                ? FindPagesByPageTypeRecursively(pageLink, pageTypeId)
                : ContentLoader.Value.GetChildren<PageData>(pageLink);
        }

        private static IEnumerable<PageData> FindPagesByPageTypeRecursively(ContentReference pageLink, int pageTypeId)
        {
            var criteria = new PropertyCriteriaCollection
            {
                new PropertyCriteria
                {
                    Name = "PageTypeID",
                    Type = PropertyDataType.PageType,
                    Condition = CompareCondition.Equal,
                    Value = pageTypeId.ToString(CultureInfo.InvariantCulture)
                }
            };

            if (!ProviderManager.Value.ProviderMap.CustomProvidersExist)
            {
                return PageCriteriaQueryService.Value.FindPagesWithCriteria(pageLink.ToPageReference(), criteria);
            }

            var contentProvider = ProviderManager.Value.ProviderMap.GetProvider(pageLink);
            if (contentProvider.HasCapability(ContentProviderCapabilities.Search))
            {
                criteria.Add(new PropertyCriteria
                {
                    Name = "EPI:MultipleSearch",
                    Value = contentProvider.ProviderKey
                });
            }

            return PageCriteriaQueryService.Value.FindPagesWithCriteria(pageLink.ToPageReference(), criteria);
        }

        /// <summary>
        /// Helper method to get a URL string for a content reference using the PreferredCulture
        /// </summary>
        /// <param name="contentRef">The content reference of a routable content item to get the URL for.</param>
        /// <param name="isAbsolute">Whether the full URL including protocol and host should be returned.</param>
        public static Uri GetUri(this ContentReference contentRef, bool isAbsolute = false) => contentRef.GetUri(ContentLanguage.PreferredCulture.Name, isAbsolute);

        /// <summary>
        /// Helper method to get a URL string for a content reference using the provided culture code
        /// </summary>
        /// <param name="contentRef">The content reference of a routable content item to get the URL for.</param>
        /// <param name="lang">The language code to use when retrieving the URL.</param>
        /// <param name="isAbsolute">Whether the full URL including protocol and host should be returned.</param>
        public static Uri GetUri(this ContentReference contentRef, string lang, bool isAbsolute = false)
        {
            var urlString = UrlResolver.Value.GetUrl(contentRef, lang, new UrlResolverArguments { ForceCanonical = true });
            if (string.IsNullOrEmpty(urlString))
            {
                return new Uri(string.Empty);
            }

            //if we're not getting an absolute URL, we don't need to work out the correct host name so exit here
            var uri = new Uri(urlString, UriKind.RelativeOrAbsolute);
            if (uri.IsAbsoluteUri || !isAbsolute)
            {
                return uri;
            }

            //Work out the correct domain to use from the hosts defined in the site definition
            var siteDefinition = SiteDefinitionResolver.Value.GetByContent(contentRef, true, true);
            var host = siteDefinition.Hosts.FirstOrDefault(h => h.Type == HostDefinitionType.Primary) ?? siteDefinition.Hosts.FirstOrDefault(h => h.Type == HostDefinitionType.Undefined);
            Uri baseUrl;

            if (host != null)
            {
                baseUrl = (host?.Name ?? "*").Equals("*") ? siteDefinition.SiteUrl : new Uri($"http{((host.UseSecureConnection ?? false) ? "s" : string.Empty)}://{host.Name}");
            }
            else
            {
                var siteDef = SiteDefinitionResolver.Value.GetByContent(ContentReference.StartPage, true, true);
                var siteDefHost = siteDef.Hosts.FirstOrDefault(h => h.Type == HostDefinitionType.Primary) ?? siteDefinition.Hosts.FirstOrDefault(h => h.Type == HostDefinitionType.Undefined);
                baseUrl = (siteDefHost?.Name ?? "*").Equals("*") ? siteDef.SiteUrl : new Uri($"http{((siteDefHost.UseSecureConnection ?? false) ? "s" : string.Empty)}://{siteDefHost.Name}");
            }

            return new Uri(baseUrl, urlString);
        }
    }
}