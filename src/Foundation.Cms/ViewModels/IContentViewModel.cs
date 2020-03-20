using EPiServer.Core;
using EPiServer.Web;
using Foundation.Cms.Pages;
using System.Collections.Generic;
using System.Globalization;
using System.Web;

namespace Foundation.Cms.ViewModels
{
    public interface IContentViewModel<out TContent> where TContent : IContent
    {
        TContent CurrentContent { get; }
        CmsHomePage StartPage { get; }
        HtmlString SchemaMarkup { get; }
        List<SiteDefinition> SiteDefinitions();
        IEnumerable<KeyValuePair<CultureInfo, string>> CurrentPageLanguages();
    }
}