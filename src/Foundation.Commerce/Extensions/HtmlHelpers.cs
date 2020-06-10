using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.ServiceLocation;
using EPiServer.SpecializedProperties;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Commerce.Models.Catalog;
using Foundation.Commerce.Models.Pages;
using System;
using System.Linq;
using System.Net;
using System.Text;
using System.Web.Mvc;

namespace Foundation.Commerce.Extensions
{
    public static class HtmlHelpers
    {
        private const string _cssFormat = "<link href=\"{0}\" rel=\"stylesheet\" />";
        private const string _scriptFormat = "<script src=\"{0}\"></script>";
        private const string _metaFormat = "<meta property=\"{0}\" content=\"{1}\" />";

        private static readonly Lazy<IContentLoader> _contentLoader =
            new Lazy<IContentLoader>(() => ServiceLocator.Current.GetInstance<IContentLoader>());

        private static readonly Lazy<IUrlResolver> _urlResolver =
           new Lazy<IUrlResolver>(() => ServiceLocator.Current.GetInstance<IUrlResolver>());

        private static readonly Lazy<IPermanentLinkMapper> _permanentLinkMapper =
           new Lazy<IPermanentLinkMapper>(() => ServiceLocator.Current.GetInstance<IPermanentLinkMapper>());

        public static MvcHtmlString RenderExtendedCssForCommerce(this HtmlHelper helper, IContent content)
        {
            if (content == null || ContentReference.StartPage == PageReference.EmptyReference || !(content is IFoundationContent entryContentBase))
            {
                return new MvcHtmlString("");
            }

            var outputCss = new StringBuilder(string.Empty);
            var startPage = _contentLoader.Value.Get<CommerceHomePage>(ContentReference.StartPage);

            // Extended Css file
            AppendFiles(startPage.CssFiles, outputCss, _cssFormat);
            AppendFiles(entryContentBase.CssFiles, outputCss, _cssFormat);

            // Inline CSS
            if (!string.IsNullOrWhiteSpace(startPage.Scripts) || !string.IsNullOrWhiteSpace(entryContentBase.Scripts))
            {
                outputCss.AppendLine("<style>");
                outputCss.AppendLine(!string.IsNullOrWhiteSpace(startPage.Css) ? startPage.Css : "");
                outputCss.AppendLine(!string.IsNullOrWhiteSpace(entryContentBase.Css) ? entryContentBase.Css : "");
                outputCss.AppendLine("</style>");
            }

            return new MvcHtmlString(outputCss.ToString());
        }

        public static MvcHtmlString RenderExtendedScriptsForCommerce(this HtmlHelper helper, IContent content)
        {
            if (content == null || ContentReference.StartPage == PageReference.EmptyReference || !(content is IFoundationContent entryContentBase))
            {
                return new MvcHtmlString("");
            }

            var outputScript = new StringBuilder(string.Empty);
            var startPage = _contentLoader.Value.Get<CommerceHomePage>(ContentReference.StartPage);

            // Extended Javascript file
            AppendFiles(startPage.ScriptFiles, outputScript, _scriptFormat);
            AppendFiles(entryContentBase.ScriptFiles, outputScript, _scriptFormat);

            // Inline Javascript
            if (!string.IsNullOrWhiteSpace(startPage.Scripts) || !string.IsNullOrWhiteSpace(entryContentBase.Scripts))
            {
                outputScript.AppendLine("<script type=\"text/javascript\">");
                outputScript.AppendLine(!string.IsNullOrWhiteSpace(startPage.Scripts) ? startPage.Scripts : "");
                outputScript.AppendLine(!string.IsNullOrWhiteSpace(entryContentBase.Scripts) ? entryContentBase.Scripts : "");
                outputScript.AppendLine("</script>");
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
    }
}