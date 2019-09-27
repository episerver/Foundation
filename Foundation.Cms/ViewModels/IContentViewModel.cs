using EPiServer.Core;
using Foundation.Cms.Pages;

namespace Foundation.Cms.ViewModels
{
    public interface IContentViewModel<out TContent> where TContent : IContent
    {
        TContent CurrentContent { get; }
        CmsHomePage StartPage { get; }
    }
}