using EPiServer;
using EPiServer.Core;
using EPiServer.Editor;
using EPiServer.Globalization;
using EPiServer.ServiceLocation;
using Foundation.Cms.Pages;

namespace Foundation.Cms.ViewModels
{
    public class ContentViewModel<TContent> : IContentViewModel<TContent> where TContent : IContent
    {
        private Injected<IContentLoader> _contentLoader;
        private Injected<IContentVersionRepository> _contentVersion;
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
                    if (PageEditing.PageIsInEditMode)
                    {
                        var startPageRef = _contentVersion.Service.LoadCommonDraft(ContentReference.StartPage, ContentLanguage.PreferredCulture.Name);
                        _startPage = _contentLoader.Service.Get<CmsHomePage>(startPageRef.ContentLink);
                    }
                    else
                    {
                        _startPage = _contentLoader.Service.Get<CmsHomePage>(ContentReference.StartPage);
                    }
                }

                return _startPage;
            }
        }
    }

    public static class ContentViewModel
    {
        public static ContentViewModel<T> Create<T>(T content) where T : IContent => new ContentViewModel<T>(content);
    }
}