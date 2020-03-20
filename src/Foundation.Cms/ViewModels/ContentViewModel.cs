using EPiServer;
using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Foundation.Cms.Extensions;
using Foundation.Cms.Pages;
using Foundation.Cms.SchemaMarkup;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Foundation.Cms.ViewModels
{
    public class ContentViewModel<TContent> : IContentViewModel<TContent> where TContent : IContent
    {
        private Injected<IContentLoader> _contentLoader;
        private Injected<IContentVersionRepository> _contentVersion;
        private Injected<ISiteDefinitionRepository> _siteDefinitionRepository;
        private Injected<UrlResolver> _urlResolver;
        private CmsHomePage _startPage;

        public ContentViewModel() : this(default)
        {
        }

        public ContentViewModel(TContent currentContent)
        {
            CurrentContent = currentContent;
        }

        public TContent CurrentContent { get; set; }

        public virtual CmsHomePage StartPage
        {
            get
            {
                if (_startPage == null)
                {
                    ContentReference currentStartPageLink = ContentReference.StartPage;
                    if (CurrentContent != null)
                    {
                        currentStartPageLink = CurrentContent.GetRelativeStartPage();
                    }

                    if (PageEditing.PageIsInEditMode)
                    {
                        var startPageRef = _contentVersion.Service.LoadCommonDraft(currentStartPageLink, ContentLanguage.PreferredCulture.Name);
                        if (startPageRef == null)
                        {
                            _startPage = _contentLoader.Service.Get<CmsHomePage>(currentStartPageLink);
                        }
                        else
                        {
                            _startPage = _contentLoader.Service.Get<CmsHomePage>(startPageRef.ContentLink);
                        }
                    }
                    else
                    {
                        _startPage = _contentLoader.Service.Get<CmsHomePage>(currentStartPageLink);
                    }
                }

                return _startPage;
            }
        }

        public HtmlString SchemaMarkup
        {
            get
            {
                //See if there's a schema data mapper for this content type and, if so, generate some schema markup
                if (ServiceLocator.Current.TryGetExistingInstance(out ISchemaDataMapper<TContent> mapper))
                {
                    return new HtmlString($"<script type=\"application/ld+json\">{mapper.Map(CurrentContent).ToHtmlEscapedString()}</script>");
                }
                return new HtmlString(string.Empty);
            }
        }

        public List<SiteDefinition> SiteDefinitions()
        {
            var siteDefinitions = _siteDefinitionRepository.Service.List().ToList();
            return siteDefinitions;
        }

        public IEnumerable<KeyValuePair<CultureInfo, string>> CurrentPageLanguages()
        {
            var page = CurrentContent as PageData;
            if (page != null)
            {
                var existLanguages = page.ExistingLanguages;
                foreach(var language in existLanguages)
                {
                    yield return new KeyValuePair<CultureInfo, string>(language, _urlResolver.Service.GetUrl(CurrentContent.ContentLink, language.Name));
                }
            }
        }
    }

    public static class ContentViewModel
    {
        public static ContentViewModel<T> Create<T>(T content) where T : IContent => new ContentViewModel<T>(content);
    }
}