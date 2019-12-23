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
                    var currentPage = CurrentContent as FoundationPageData;
                    var currentStartPageLink = ContentReference.StartPage;
                    if (currentPage != null)
                    {
                        if (!ContentReference.IsNullOrEmpty(currentPage.StartPageLink))
                        {
                            currentStartPageLink = currentPage.StartPageLink;
                        }

                        if (currentPage is CmsHomePage)
                        {
                            currentStartPageLink = currentPage.PageLink;
                        }
                    }

                    if (PageEditing.PageIsInEditMode)
                    {
                        var startPageRef = _contentVersion.Service.LoadCommonDraft(currentStartPageLink, ContentLanguage.PreferredCulture.Name);
                        _startPage = _contentLoader.Service.Get<CmsHomePage>(startPageRef.ContentLink);
                    }
                    else
                    {
                        _startPage = _contentLoader.Service.Get<CmsHomePage>(currentStartPageLink);
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