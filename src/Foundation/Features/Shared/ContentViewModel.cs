using EPiServer.Globalization;
using Foundation.Features.Home;
using Foundation.Infrastructure.Cms;

namespace Foundation.Features.Shared
{
    public class ContentViewModel<TContent> : IContentViewModel<TContent> where TContent : IContent
    {
        private Injected<IContentLoader> _contentLoader;
        private Injected<IContentVersionRepository> _contentVersion;
        private Injected<IContextModeResolver> _contextModeResolver;
        private HomePage _startPage;

        public ContentViewModel() : this(default)
        {
        }

        public ContentViewModel(TContent currentContent)
        {
            CurrentContent = currentContent;
        }

        public TContent CurrentContent { get; set; }

        public virtual HomePage StartPage
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

                    if (_contextModeResolver.Service.CurrentMode == ContextMode.Edit)
                    {
                        var startPageRef = _contentVersion.Service.LoadCommonDraft(currentStartPageLink, ContentLanguage.PreferredCulture.Name);
                        if (startPageRef == null)
                        {
                            _startPage = _contentLoader.Service.Get<HomePage>(currentStartPageLink);
                        }
                        else
                        {
                            _startPage = _contentLoader.Service.Get<HomePage>(startPageRef.ContentLink);
                        }
                    }
                    else
                    {
                        _startPage = _contentLoader.Service.Get<HomePage>(currentStartPageLink);
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
    }

    public static class ContentViewModel
    {
        public static ContentViewModel<T> Create<T>(T content) where T : IContent => new ContentViewModel<T>(content);
    }
}