using Boilerplate.Web.Mvc.OpenGraph;
using EPiServer;
using EPiServer.Commerce.Catalog;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Foundation.Cms.Pages;
using Foundation.Demo.Models;
using Foundation.Find.Cms.Models.Pages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Foundation.Helpers
{
    public static class HtmlHelpers
    {
        private static readonly Lazy<IContentLoader> _contentLoader = new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());
        private static readonly Lazy<AssetUrlResolver> _assetUrlResolver = new Lazy<AssetUrlResolver>(() => ServiceLocator.Current.GetInstance<AssetUrlResolver>());
        private static readonly Lazy<IContentTypeRepository> _contentTypeRepository = new Lazy<IContentTypeRepository>(() => ServiceLocator.Current.GetInstance<IContentTypeRepository>());
        private const string MetaFormat = "<meta property=\"{0}\" content=\"{1}\" />";

        public static IHtmlString RenderOpenGraphMetaData(this HtmlHelper helper, IContent content)
        {
            string openGraphOutput = string.Empty;
            string metaTitle = ((FoundationPageData)content).MetaTitle ?? content.Name;
            string imageUrl;
            
            if (((FoundationPageData)content).PageImage != null)
            {
                imageUrl = GetUrl(((FoundationPageData)content).PageImage);
            }
            else
            {
                imageUrl = GetDefaultImageUrl();
            }

            switch (content)
            {
                case DemoHomePage homePage:
                    var openGraphWebsite = new OpenGraphWebsite(metaTitle, new OpenGraphImage(imageUrl), GetUrl(homePage.ContentLink))
                    {
                        Description = homePage.PageDescription
                    };

                    openGraphOutput = helper.OpenGraph(openGraphWebsite).ToString();
                    break;

                case BlogItemPage _:
                case StandardPage _:
                case LocationItemPage _:
                case TagPage _:
                    var openGraphArticle = new OpenGraphArticle(metaTitle, new OpenGraphImage(imageUrl), GetUrl(content.ContentLink))
                    {
                        Description = ((FoundationPageData)content).PageDescription,
                    };

                    if (content is LocationItemPage)
                    {
                        var tags = new List<string>();
                        foreach (var item in ((LocationItemPage)content).Tags.Items)
                        {
                            tags.Add(item.GetContent().Name);
                        }
                        openGraphArticle.Tags = tags;
                    }

                    if (((FoundationPageData)content).AuthorMetaData != null)
                    {
                        openGraphArticle.AuthorUrls = new List<string> { ((FoundationPageData)content).AuthorMetaData };
                    }

                    if (((FoundationPageData)content).StartPublish.HasValue)
                    {
                        openGraphArticle.PublishedTime = ((FoundationPageData)content).StartPublish.Value;
                    }

                    openGraphArticle.ModifiedTime = ((FoundationPageData)content).Changed;

                    if (((FoundationPageData)content).StopPublish.HasValue)
                    {
                        openGraphArticle.ExpirationTime = ((FoundationPageData)content).StopPublish.Value;
                    }

                    openGraphOutput = helper.OpenGraph(openGraphArticle).ToString();
                    break;

                case FoundationPageData foundationPageData:
                    var openGraphPage = new OpenGraphArticle(metaTitle, new OpenGraphImage(imageUrl), GetUrl(foundationPageData.ContentLink))
                    {
                        Description = foundationPageData.PageDescription,
                    };

                    if (foundationPageData.AuthorMetaData != null)
                    {
                        openGraphPage.AuthorUrls = new List<string> { foundationPageData.AuthorMetaData };
                    }

                    if (foundationPageData.StartPublish.HasValue)
                    {
                        openGraphPage.PublishedTime = foundationPageData.StartPublish.Value;
                    }

                    openGraphPage.ModifiedTime = foundationPageData.Changed;

                    if (foundationPageData.StopPublish.HasValue)
                    {
                        openGraphPage.ExpirationTime = foundationPageData.StopPublish.Value;
                    }

                    openGraphOutput = helper.OpenGraph(openGraphPage).ToString();
                    break;

                case EntryContentBase entryContentBase:
                    var openGraphEntry = new OpenGraphProduct(entryContentBase.DisplayName, new OpenGraphImage(_assetUrlResolver.Value.GetAssetUrl(entryContentBase)), GetUrl(entryContentBase.ContentLink));
                    
                    openGraphOutput = helper.OpenGraph(openGraphEntry).ToString();
                    break;
            }

            var output = new StringBuilder();
            output.AppendLine(openGraphOutput);
            output.AppendLine(AddAdditionalOpenGraphMetaData(content));

            return new HtmlString(output.ToString());
        }

        public static string AddAdditionalOpenGraphMetaData(IContent content)
        {
            var output = new StringBuilder(string.Empty);

            if (content is LocationItemPage && ((LocationItemPage)content).Continent != null)
            {
                output.AppendLine(string.Format(MetaFormat, "article:category", ((LocationItemPage)content).Continent));
            }

            if (content is FoundationPageData)
            {
                if (((FoundationPageData)content).ContentType != null)
                {
                    output.AppendLine(string.Format(MetaFormat, "article:content_type", ((FoundationPageData)content).ContentType));
                }
                else
                {
                    var pageType = _contentTypeRepository.Value.Load(content.GetOriginalType());
                    output.AppendLine(string.Format(MetaFormat, "article:content_type", pageType.DisplayName));
                }

                if (((FoundationPageData)content).Industry != null)
                {
                    output.AppendLine(string.Format(MetaFormat, "article:industry", ((FoundationPageData)content).Industry));
                }

                if (((FoundationPageData)content).AuthorMetaData != null)
                {
                    output.AppendLine(string.Format(MetaFormat, "article:author", ((FoundationPageData)content).AuthorMetaData));
                }
            }

            if (content is EntryContentBase)
            {
                if (((EntryContentBase)content).Categories != null)
                {
                    output.AppendLine(string.Format(MetaFormat, "article:category", ((EntryContentBase)content).Categories));
                }
            }

            return output.ToString();
        }

        private static string GetDefaultImageUrl()
        {
            var startPage = _contentLoader.Value.Get<DemoHomePage>(ContentReference.StartPage);
            var siteUrl = SiteDefinition.Current.SiteUrl;
            var url = new Uri(siteUrl, UrlResolver.Current.GetUrl(startPage.SiteLogo));

            return url.ToString();
        }

        private static string GetUrl(ContentReference content)
        {
            var siteUrl = SiteDefinition.Current.SiteUrl;
            var url = new Uri(siteUrl, UrlResolver.Current.GetUrl(content));

            return url.ToString();
        }
    }
}
