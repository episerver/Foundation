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
                if (PageEditing.PageIsInEditMode)
                {
                    var startPageRef = _contentVersion.Service.LoadCommonDraft(ContentReference.StartPage, ContentLanguage.PreferredCulture.Name);
                    return _contentLoader.Service.Get<CmsHomePage>(startPageRef.ContentLink);
                }
                else
                {
                    return _contentLoader.Service.Get<CmsHomePage>(ContentReference.StartPage);
                }
            }
        }
    }

    public static class ContentViewModel
    {
        public static ContentViewModel<T> Create<T>(T content) where T : IContent => new ContentViewModel<T>(content);
    }
}