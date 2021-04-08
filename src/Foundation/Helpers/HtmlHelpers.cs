using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using EPiServer.Web.Mvc.Html;
using EPiServer.Web.Routing;
using Foundation.Cms.Extensions;
using Foundation.Cms.Settings;
using Foundation.Features.CatalogContent.Bundle;
using Foundation.Features.CatalogContent.Package;
using Foundation.Features.CatalogContent.Product;
using Foundation.Features.CatalogContent.Variation;
using Foundation.Features.Home;
using Foundation.Features.Settings;
using Foundation.Features.Shared;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace Foundation.Infrastructure
{
    public static class HtmlHelpers
    {
        private const string _cssFormat = "<link href=\"{0}\" rel=\"stylesheet\" />";
        private const string _scriptFormat = "<script src=\"{0}\"></script>";
        private const string _metaFormat = "<meta name=\"{0}\" property=\"{0}\" content=\"{1}\" />";

        private static readonly Lazy<IContentLoader> _contentLoader =
            new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        private static readonly Lazy<IUrlResolver> _urlResolver =
           new Lazy<IUrlResolver>(() => ServiceLocator.Current.GetInstance<IUrlResolver>());

        private static readonly Lazy<IPermanentLinkMapper> _permanentLinkMapper =
           new Lazy<IPermanentLinkMapper>(() => ServiceLocator.Current.GetInstance<IPermanentLinkMapper>());

        private static readonly Lazy<ISettingsService> _settingsService =
           new Lazy<ISettingsService>(() => ServiceLocator.Current.GetInstance<ISettingsService>());

        public static MvcHtmlString RenderExtendedCss(this HtmlHelper helper, IContent content)
        {
            if (content == null || ContentReference.StartPage == PageReference.EmptyReference || !(content is IFoundationContent sitePageData))
            {
                return new MvcHtmlString("");
            }

            var outputCss = new StringBuilder(string.Empty);
            var startPage = _contentLoader.Value.Get<HomePage>(ContentReference.StartPage);

            // Extended Css file
            AppendFiles(startPage.CssFiles, outputCss, _cssFormat);
            if (!(sitePageData is HomePage))
            {
                AppendFiles(sitePageData.CssFiles, outputCss, _cssFormat);
            }

            // Inline CSS
            if (!string.IsNullOrWhiteSpace(startPage.Css) || !string.IsNullOrWhiteSpace(sitePageData.Css))
            {
                outputCss.AppendLine("<style>");
                outputCss.AppendLine(!string.IsNullOrWhiteSpace(startPage.Css) ? startPage.Css : "");
                outputCss.AppendLine(!string.IsNullOrWhiteSpace(sitePageData.Css) && !(sitePageData is HomePage) ? sitePageData.Css : "");
                outputCss.AppendLine("</style>");
            }

            return new MvcHtmlString(outputCss.ToString());
        }

        public static MvcHtmlString RenderHeaderScripts(this HtmlHelper helper, IContent content)
        {
            if (content == null || ContentReference.StartPage == PageReference.EmptyReference || !(content is FoundationPageData sitePageData))
            {
                return new MvcHtmlString("");
            }

            var outputScript = new StringBuilder(string.Empty);

            // Injection Hierarchically Javascript files
            var settings = _settingsService.Value.GetSiteSettings<ScriptInjectionSettings>();
            if (settings != null && settings.HeaderScripts != null)
            {
                foreach (var script in settings.HeaderScripts)
                {
                    var pages = _contentLoader.Value.GetDescendents(script.ScriptRoot);
                    if (pages.Any(x => content.ContentLink.ID == x.ID) || content.ContentLink.ID == script.ScriptRoot.ID)
                    {
                        // Script Files
                        AppendFiles(script.ScriptFiles, outputScript, _scriptFormat);

                        // External Javascript
                        if (!string.IsNullOrWhiteSpace(script.ExternalScripts))
                        {
                            outputScript.AppendLine(script.ExternalScripts);
                        }

                        // Inline Javascript
                        if (!string.IsNullOrWhiteSpace(script.InlineScripts))
                        {
                            outputScript.AppendLine("<script type=\"text/javascript\">");
                            outputScript.AppendLine(!string.IsNullOrWhiteSpace(script.InlineScripts) ? script.InlineScripts : "");
                            outputScript.AppendLine("</script>");
                        }
                    }
                }
            }

            return new MvcHtmlString(outputScript.ToString());
        }

        public static MvcHtmlString RenderFooterScripts(this HtmlHelper helper, IContent content)
        {
            if (content == null || ContentReference.StartPage == PageReference.EmptyReference || !(content is FoundationPageData sitePageData))
            {
                return new MvcHtmlString("");
            }

            var outputScript = new StringBuilder(string.Empty);

            // Injection Hierarchically Javascript files
            var settings = _settingsService.Value.GetSiteSettings<ScriptInjectionSettings>();
            if (settings != null && settings.FooterScripts != null)
            {
                foreach (var script in settings.FooterScripts)
                {
                    var pages = _contentLoader.Value.GetDescendents(script.ScriptRoot);
                    if (pages.Any(x => content.ContentLink.ID == x.ID) || content.ContentLink.ID == script.ScriptRoot.ID)
                    {
                        // Script Files
                        AppendFiles(script.ScriptFiles, outputScript, _scriptFormat);

                        // External Javascript
                        if (!string.IsNullOrWhiteSpace(script.ExternalScripts))
                        {
                            outputScript.AppendLine(script.ExternalScripts);
                        }

                        // Inline Javascript
                        if (!string.IsNullOrWhiteSpace(script.InlineScripts))
                        {
                            outputScript.AppendLine("<script type=\"text/javascript\">");
                            outputScript.AppendLine(!string.IsNullOrWhiteSpace(script.InlineScripts) ? script.InlineScripts : "");
                            outputScript.AppendLine("</script>");
                        }
                    }
                }
            }

            return new MvcHtmlString(outputScript.ToString());
        }

        public static MvcHtmlString RenderMetaData(this HtmlHelper helper, IContent content)
        {
            if (content == null || !(content is FoundationPageData sitePageData))
            {
                return new MvcHtmlString("");
            }

            var output = new StringBuilder(string.Empty);

            if (!string.IsNullOrWhiteSpace(sitePageData.MetaTitle))
            {
                output.AppendLine(string.Format(_metaFormat, "title", sitePageData.MetaTitle));
            }
            if (!string.IsNullOrEmpty(sitePageData.Keywords))
            {
                output.AppendLine(string.Format(_metaFormat, "keywords", sitePageData.Keywords));
            }
            if (!string.IsNullOrWhiteSpace(sitePageData.PageDescription))
            {
                output.AppendLine(string.Format(_metaFormat, "description", sitePageData.PageDescription));
            }
            if (sitePageData.DisableIndexing)
            {
                output.AppendLine("<meta name=\"robots\" content=\"NOINDEX, NOFOLLOW\">");
            }

            return new MvcHtmlString(output.ToString());
        }

        public static MvcHtmlString RenderExtendedCssForCommerce(this HtmlHelper helper, IContent content)
        {
            if (content == null || ContentReference.StartPage == PageReference.EmptyReference || !(content is EntryContentBase entryContentBase))
            {
                return new MvcHtmlString("");
            }

            var outputCss = new StringBuilder(string.Empty);
            var startPage = _contentLoader.Value.Get
                <HomePage>(ContentReference.StartPage);

            // Extended Css file
            AppendFiles(startPage.CssFiles, outputCss, _cssFormat);
            AppendFiles(((IFoundationContent)entryContentBase).CssFiles, outputCss, _cssFormat);

            // Inline CSS
            if (!string.IsNullOrWhiteSpace(startPage.Css) || !string.IsNullOrWhiteSpace(((IFoundationContent)entryContentBase).Css))
            {
                outputCss.AppendLine("<style>");
                outputCss.AppendLine(!string.IsNullOrWhiteSpace(startPage.Css) ? startPage.Css : "");
                outputCss.AppendLine(!string.IsNullOrWhiteSpace(((IFoundationContent)entryContentBase).Css) ? ((IFoundationContent)entryContentBase).Css : "");
                outputCss.AppendLine("</style>");
            }

            return new MvcHtmlString(outputCss.ToString());
        }

        public static MvcHtmlString RenderHeaderScriptsForCommerce(this HtmlHelper helper, IContent content)
        {
            if (content == null || ContentReference.StartPage == PageReference.EmptyReference || !(content is EntryContentBase || content is CatalogContentBase))
            {
                return new MvcHtmlString("");
            }

            var outputScript = new StringBuilder(string.Empty);

            // Injection Hierarchically Javascript
            var settings = _settingsService.Value.GetSiteSettings<ScriptInjectionSettings>();
            if (settings != null && settings.HeaderScripts != null)
            {
                foreach (var script in settings.HeaderScripts)
                {
                    var pages = _contentLoader.Value.GetDescendents(script.ScriptRoot);
                    if (pages.Any(x => content.ContentLink.ID == x.ID) || content.ContentLink.ID == script.ScriptRoot.ID)
                    {
                        // Script Files
                        AppendFiles(script.ScriptFiles, outputScript, _scriptFormat);

                        // External Javascript
                        if (!string.IsNullOrWhiteSpace(script.ExternalScripts))
                        {
                            outputScript.AppendLine(script.ExternalScripts);
                        }

                        // Inline Javascript
                        if (!string.IsNullOrWhiteSpace(script.InlineScripts))
                        {
                            outputScript.AppendLine("<script type=\"text/javascript\">");
                            outputScript.AppendLine(!string.IsNullOrWhiteSpace(script.InlineScripts) ? script.InlineScripts : "");
                            outputScript.AppendLine("</script>");
                        }
                    }
                }
            }

            return new MvcHtmlString(outputScript.ToString());
        }

        public static MvcHtmlString RenderFooterScriptsForCommerce(this HtmlHelper helper, IContent content)
        {
            if (content == null || ContentReference.StartPage == PageReference.EmptyReference || !(content is EntryContentBase || content is CatalogContentBase))
            {
                return new MvcHtmlString("");
            }

            var outputScript = new StringBuilder(string.Empty);

            // Injection Hierarchically Javascript
            var settings = _settingsService.Value.GetSiteSettings<ScriptInjectionSettings>();
            if (settings != null && settings.FooterScripts != null)
            {
                foreach (var script in settings.FooterScripts)
                {
                    var pages = _contentLoader.Value.GetDescendents(script.ScriptRoot);
                    if (pages.Any(x => content.ContentLink.ID == x.ID) || content.ContentLink.ID == script.ScriptRoot.ID)
                    {
                        // Script Files
                        AppendFiles(script.ScriptFiles, outputScript, _scriptFormat);

                        // External Javascript
                        if (!string.IsNullOrWhiteSpace(script.ExternalScripts))
                        {
                            outputScript.AppendLine(script.ExternalScripts);
                        }

                        // Inline Javascript
                        if (!string.IsNullOrWhiteSpace(script.InlineScripts))
                        {
                            outputScript.AppendLine("<script type=\"text/javascript\">");
                            outputScript.AppendLine(!string.IsNullOrWhiteSpace(script.InlineScripts) ? script.InlineScripts : "");
                            outputScript.AppendLine("</script>");
                        }
                    }
                }
            }

            return new MvcHtmlString(outputScript.ToString());
        }

        public static MvcHtmlString RenderMetaDataForCommerce(this HtmlHelper helper, IContent content)
        {
            if (content == null || !(content is EntryContentBase entryContentBase))
            {
                return new MvcHtmlString("");
            }

            var output = new StringBuilder(string.Empty);

            if (!string.IsNullOrWhiteSpace(entryContentBase.SeoInformation.Title))
            {
                output.AppendLine(string.Format(_metaFormat, "title", entryContentBase.SeoInformation.Title));
            }

            if (!string.IsNullOrWhiteSpace(entryContentBase.SeoInformation.Keywords))
            {
                output.AppendLine(string.Format(_metaFormat, "keyword", entryContentBase.SeoInformation.Keywords));
            }

            if (!string.IsNullOrWhiteSpace(entryContentBase.SeoInformation.Description))
            {
                output.AppendLine(string.Format(_metaFormat, "description", entryContentBase.SeoInformation.Description));
            }
            else
            {
                switch (entryContentBase)
                {
                    case GenericProduct genericProduct:
                        output.AppendLine(string.Format(_metaFormat, "description",
                            genericProduct.Description != null ? WebUtility.HtmlEncode(genericProduct.Description.ToString()) : ""));
                        break;
                    case GenericVariant genericVariant:
                        output.AppendLine(string.Format(_metaFormat, "description",
                            genericVariant.Description != null ? WebUtility.HtmlEncode(genericVariant.Description.ToString()) : ""));
                        break;
                    case GenericPackage genericPackage:
                        output.AppendLine(string.Format(_metaFormat, "description",
                            genericPackage.Description != null ? WebUtility.HtmlEncode(genericPackage.Description.ToString()) : ""));
                        break;
                    case GenericBundle genericBundle:
                        output.AppendLine(string.Format(_metaFormat, "description",
                            genericBundle.Description != null ? WebUtility.HtmlEncode(genericBundle.Description.ToString()) : ""));
                        break;
                    default:
                        break;
                }
            }

            return new MvcHtmlString(output.ToString());
        }

        //public static ContentReference GetSearchPage(this HtmlHelper helper) => ContentLoader.Value.Get<CmsHomePage>(ContentReference.StartPage).SearchPage;

        private static void AppendFiles(LinkItemCollection files, StringBuilder outputString, string formatString)
        {
            if (files == null || files.Count <= 0)
            {
                return;
            }

            foreach (var item in files.Where(item => !string.IsNullOrEmpty(item.Href)))
            {
                var map = _permanentLinkMapper.Value.Find(new UrlBuilder(item.Href));
                outputString.AppendLine(map == null
                    ? string.Format(formatString, item.GetMappedHref())
                    : string.Format(formatString, _urlResolver.Value.GetUrl(map.ContentReference)));
            }
        }

        private static void AppendFiles(IList<ContentReference> files, StringBuilder outputString, string formatString)
        {
            if (files == null || files.Count <= 0)
            {
                return;
            }

            foreach (var item in files.Where(item => !string.IsNullOrEmpty(_urlResolver.Value.GetUrl(item))))
            {
                var url = _urlResolver.Value.GetUrl(item);
                outputString.AppendLine(string.Format(formatString, url));
            }
        }

        public static ConditionalLink BeginConditionalLink(this HtmlHelper helper, bool shouldWriteLink,
            IHtmlString url, string title = null, string cssClass = null)
        {
            if (shouldWriteLink)
            {
                var linkTag = new TagBuilder("a");
                linkTag.Attributes.Add("href", url.ToHtmlString());

                if (!string.IsNullOrWhiteSpace(title))
                {
                    linkTag.Attributes.Add("title", helper.Encode(title));
                }

                if (!string.IsNullOrWhiteSpace(cssClass))
                {
                    linkTag.Attributes.Add("class", cssClass);
                }

                helper.ViewContext.Writer.Write(linkTag.ToString(TagRenderMode.StartTag));
            }

            return new ConditionalLink(helper.ViewContext, shouldWriteLink);
        }

        public static ConditionalLink BeginConditionalLink(this HtmlHelper helper, bool shouldWriteLink,
            Func<IHtmlString> urlGetter, string title = null, string cssClass = null)
        {
            IHtmlString url = MvcHtmlString.Empty;

            if (shouldWriteLink)
            {
                url = urlGetter();
            }

            return helper.BeginConditionalLink(shouldWriteLink, url, title, cssClass);
        }

        public static IHtmlString MenuList(
            this HtmlHelper helper,
            ContentReference rootLink,
            Func<MenuItem, HelperResult> itemTemplate = null,
            bool includeRoot = false,
            bool requireVisibleInMenu = true,
            bool requirePageTemplate = true)
        {
            itemTemplate = itemTemplate ?? GetDefaultItemTemplate(helper);
            var currentContentLink = helper.ViewContext.RequestContext.GetContentLink();

            Func<IEnumerable<PageData>, IEnumerable<PageData>> filter =
                pages => pages.FilterForDisplay(requirePageTemplate, requireVisibleInMenu);

            var pagePath = _contentLoader.Value.GetAncestors(currentContentLink)
                .Reverse()
                .Select(x => x.ContentLink)
                .SkipWhile(x => !x.CompareToIgnoreWorkID(rootLink))
                .ToList();

            var menuItems = _contentLoader.Value.GetChildren<PageData>(rootLink)
                .FilterForDisplay(requirePageTemplate, requireVisibleInMenu)
                .Select(x => CreateMenuItem(x, currentContentLink, pagePath, _contentLoader.Value, filter))
                .ToList();

            if (includeRoot)
            {
                menuItems.Insert(0,
                    CreateMenuItem(_contentLoader.Value.Get<PageData>(rootLink), currentContentLink, pagePath, _contentLoader.Value,
                        filter));
            }

            var buffer = new StringBuilder();
            var writer = new StringWriter(buffer);
            foreach (var menuItem in menuItems)
            {
                itemTemplate(menuItem).WriteTo(writer);
            }

            return new MvcHtmlString(buffer.ToString());
        }

        private static MenuItem CreateMenuItem(PageData page, ContentReference currentContentLink,
            List<ContentReference> pagePath, IContentLoader contentLoader,
            Func<IEnumerable<PageData>, IEnumerable<PageData>> filter)
        {
            var menuItem = new MenuItem(page)
            {
                Selected = page.ContentLink.CompareToIgnoreWorkID(currentContentLink) ||
                           pagePath.Contains(page.ContentLink),
                HasChildren =
                    new Lazy<bool>(() => filter(contentLoader.GetChildren<PageData>(page.ContentLink)).Any())
            };
            return menuItem;
        }

        private static Func<MenuItem, HelperResult> GetDefaultItemTemplate(HtmlHelper helper) => x => new HelperResult(writer => writer.Write(helper.PageLink(x.Page)));

        public class ConditionalLink : IDisposable
        {
            private readonly bool _linked;
            private readonly ViewContext _viewContext;
            private bool _disposed;

            public ConditionalLink(ViewContext viewContext, bool isLinked)
            {
                _viewContext = viewContext;
                _linked = isLinked;
            }

            public void Dispose()
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }

            protected virtual void Dispose(bool disposing)
            {
                if (_disposed)
                {
                    return;
                }

                _disposed = true;

                if (_linked)
                {
                    _viewContext.Writer.Write("</a>");
                }
            }
        }

        public class MenuItem
        {
            public MenuItem(PageData page)
            {
                Page = page;
            }

            public PageData Page { get; set; }
            public bool Selected { get; set; }
            public Lazy<bool> HasChildren { get; set; }
        }
    }
}