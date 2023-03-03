using EPiServer.SpecializedProperties;
using Foundation.Features.CatalogContent.Bundle;
using Foundation.Features.CatalogContent.Package;
using Foundation.Features.CatalogContent.Product;
using Foundation.Features.CatalogContent.Variation;
using Foundation.Features.Home;
using Foundation.Infrastructure.Cms.Settings;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;

namespace Foundation.Infrastructure.Helpers
{
    public static class HtmlHelpers
    {
        private const string _cssFormat = "<link href=\"{0}\" rel=\"stylesheet\" />";
        private const string _scriptFormat = "<script src=\"{0}\"></script>";
        private const string _metaFormat = "<meta name=\"{0}\" property=\"{0}\" content=\"{1}\" />";

        private const string _customFontSrc = "src: url(\"{0}\")";

        private static readonly Lazy<IContentLoader> _contentLoader =
            new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        private static readonly Lazy<IUrlResolver> _urlResolver =
           new Lazy<IUrlResolver>(() => ServiceLocator.Current.GetInstance<IUrlResolver>());

        private static readonly Lazy<IPermanentLinkMapper> _permanentLinkMapper =
           new Lazy<IPermanentLinkMapper>(() => ServiceLocator.Current.GetInstance<IPermanentLinkMapper>());

        private static readonly Lazy<ISettingsService> _settingsService =
           new Lazy<ISettingsService>(() => ServiceLocator.Current.GetInstance<ISettingsService>());

        private static readonly Lazy<IContextModeResolver> _contextModeResolver =
           new Lazy<IContextModeResolver>(() => ServiceLocator.Current.GetInstance<IContextModeResolver>());

        public static HtmlString RenderExtendedCss(this IHtmlHelper helper, IContent content)
        {
            if (content == null || ContentReference.StartPage == PageReference.EmptyReference || !(content is IFoundationContent sitePageData))
            {
                return new HtmlString("");
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

            //get fonts
            var settings = _settingsService.Value.GetSiteSettings<FontSettings>();
            if (settings != null && settings.FontFields != null)
            {
                for (var i = 0; i < settings.FontFields.Count; i++)
                {
                    if (settings.FontFields[i].EnableFont)
                    {
                        outputCss.AppendLine("<style>");
                        outputCss.AppendLine(settings.FontFields[i].FontImport);
                        outputCss.AppendLine("</style>");

                        
                    }
                }

                if (!settings.GlobalFontDropDown.IsNullOrEmpty() && settings.GlobalFontDropDown != "Initial")
                {
                    outputCss.AppendLine("<style>");
                    outputCss.AppendLine("*{");
                    outputCss.AppendLine(String.Format("font-family: '{0}';", settings.GlobalFontDropDown));
                    outputCss.AppendLine("}");
                    outputCss.AppendLine("</style>");
                }
            }

            //get custom fonts
            if (settings != null && settings.CustomFonts != null)
            {
                for (var i = 0; i < settings.CustomFonts.Count; i++)
                {
                    if (settings.CustomFonts[i].EnableFont)
                    {
                        outputCss.AppendLine("<style>");
                        outputCss.AppendLine("@font-face {");
                        outputCss.AppendLine(String.Format("font-family: '{0}';", settings.CustomFonts[i].FontName));
                        AppendFiles(settings.CustomFonts[i].FontFile, outputCss, _customFontSrc);
                        outputCss.AppendLine("}");
                        outputCss.AppendLine("</style>");

                        
                    }
                }
            }

            return new HtmlString(outputCss.ToString());
        }

        public static HtmlString RenderHeaderScripts(this IHtmlHelper helper, IContent content)
        {
            if (content == null || ContentReference.StartPage == PageReference.EmptyReference || !(content is FoundationPageData sitePageData))
            {
                return new HtmlString("");
            }

            var outputScript = new StringBuilder(string.Empty);

            // Injection Hierarchically Javascript
            var settings = _settingsService.Value.GetSiteSettings<ScriptInjectionSettings>();
            if (settings != null && settings.HeaderScripts != null)
            {
                foreach (var script in settings.HeaderScripts)
                {
                    var pages = _contentLoader.Value.GetDescendents(script.ScriptRoot);
                    if (pages.Any(x => x == content.ContentLink) || content.ContentLink == script.ScriptRoot)
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

            return new HtmlString(outputScript.ToString());
        }

        public static HtmlString RenderFooterScripts(this IHtmlHelper helper, IContent content)
        {
            if (content == null || ContentReference.StartPage == PageReference.EmptyReference || !(content is FoundationPageData sitePageData))
            {
                return new HtmlString("");
            }

            var outputScript = new StringBuilder(string.Empty);

            // Injection Hierarchically Javascript
            var settings = _settingsService.Value.GetSiteSettings<ScriptInjectionSettings>();
            if (settings != null && settings.FooterScripts != null)
            {
                foreach (var script in settings.FooterScripts)
                {
                    var pages = _contentLoader.Value.GetDescendents(script.ScriptRoot);
                    if (pages.Any(x => x == content.ContentLink) || content.ContentLink == script.ScriptRoot)
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

            return new HtmlString(outputScript.ToString());
        }

        public static HtmlString RenderMetaData(this IHtmlHelper helper, IContent content)
        {
            if (content == null || !(content is FoundationPageData sitePageData))
            {
                return new HtmlString("");
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

            return new HtmlString(output.ToString());
        }

        public static HtmlString RenderExtendedCssForCommerce(this IHtmlHelper helper, IContent content)
        {
            if (content == null || ContentReference.StartPage == PageReference.EmptyReference || !(content is EntryContentBase entryContentBase))
            {
                return new HtmlString("");
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

            return new HtmlString(outputCss.ToString());
        }

        public static HtmlString RenderHeaderScriptsForCommerce(this IHtmlHelper helper, IContent content)
        {
            if (content == null || ContentReference.StartPage == PageReference.EmptyReference || !(content is EntryContentBase || content is CatalogContentBase))
            {
                return new HtmlString("");
            }

            var outputScript = new StringBuilder(string.Empty);

            // Injection Hierarchically Javascript
            var settings = _settingsService.Value.GetSiteSettings<ScriptInjectionSettings>();
            if (settings != null && settings.HeaderScripts != null)
            {
                foreach (var script in settings.HeaderScripts)
                {
                    var pages = _contentLoader.Value.GetDescendents(script.ScriptRoot);
                    if (pages.Any(x => x == content.ContentLink) || content.ContentLink == script.ScriptRoot)
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

            return new HtmlString(outputScript.ToString());
        }

        public static HtmlString RenderFooterScriptsForCommerce(this IHtmlHelper helper, IContent content)
        {
            if (content == null || ContentReference.StartPage == PageReference.EmptyReference || !(content is EntryContentBase || content is CatalogContentBase))
            {
                return new HtmlString("");
            }

            var outputScript = new StringBuilder(string.Empty);

            // Injection Hierarchically Javascript
            var settings = _settingsService.Value.GetSiteSettings<ScriptInjectionSettings>();
            if (settings != null && settings.FooterScripts != null)
            {
                foreach (var script in settings.FooterScripts)
                {
                    var pages = _contentLoader.Value.GetDescendents(script.ScriptRoot);
                    if (pages.Any(x => x == content.ContentLink) || content.ContentLink == script.ScriptRoot)
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

            return new HtmlString(outputScript.ToString());
        }

        public static HtmlString RenderMetaDataForCommerce(this IHtmlHelper helper, IContent content)
        {
            if (content == null || !(content is EntryContentBase entryContentBase))
            {
                return new HtmlString("");
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

            return new HtmlString(output.ToString());
        }

        //public static ContentReference GetSearchPage(this HtmlHelper helper) => ContentLoader.Value.Get<HomePage>(ContentReference.StartPage).SearchPage;

        private static void AppendFiles(LinkItemCollection files, StringBuilder outputString, string formatString)
        {
            if (files == null || files.Count <= 0) return;

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

        public static ConditionalLink BeginConditionalLink(this IHtmlHelper helper, bool shouldWriteLink,
            IHtmlContent url, string title = null, string cssClass = null)
        {
            if (shouldWriteLink)
            {
                var linkTag = new TagBuilder("a");
                linkTag.Attributes.Add("href", url.ToString());

                if (!string.IsNullOrWhiteSpace(title))
                {
                    linkTag.Attributes.Add("title", helper.Encode(title));
                }

                if (!string.IsNullOrWhiteSpace(cssClass))
                {
                    linkTag.Attributes.Add("class", cssClass);
                }

                helper.ViewContext.Writer.Write(linkTag.RenderSelfClosingTag());
            }

            return new ConditionalLink(helper.ViewContext, shouldWriteLink);
        }

        public static ConditionalLink BeginConditionalLink(this IHtmlHelper helper, bool shouldWriteLink,
            Func<IHtmlContent> urlGetter, string title = null, string cssClass = null)
        {
            IHtmlContent url = HtmlString.Empty;

            if (shouldWriteLink)
            {
                url = urlGetter();
            }

            return helper.BeginConditionalLink(shouldWriteLink, url, title, cssClass);
        }

        public static IHtmlContent MenuList(
            this IHtmlHelper helper,
            ContentReference rootLink,
            Func<MenuItem, HelperResult> itemTemplate = null,
            bool includeRoot = false,
            bool requireVisibleInMenu = true,
            bool requirePageTemplate = true)
        {
            itemTemplate = itemTemplate ?? GetDefaultItemTemplate(helper);
            var currentContentLink = helper.ViewContext.HttpContext.GetContentLink();

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
                itemTemplate(menuItem).WriteTo(writer, HtmlEncoder.Default);
            }

            return new HtmlString(buffer.ToString());
        }

        public static bool IsInEditMode(this IHtmlHelper htmlHelper) => _contextModeResolver.Value.CurrentMode == ContextMode.Edit;

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

        private static Func<MenuItem, HelperResult> GetDefaultItemTemplate(IHtmlHelper helper) => x => new HelperResult(async writer => await writer.WriteAsync(helper.PageLink(x.Page).ToString()));

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
            public MenuItem(PageData page) => Page = page;
            public PageData Page { get; set; }
            public bool Selected { get; set; }
            public Lazy<bool> HasChildren { get; set; }
        }
    }
}