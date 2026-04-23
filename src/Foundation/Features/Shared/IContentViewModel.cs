using Foundation.Features.Home;

namespace Foundation.Features.Shared
{
    public interface IContentViewModel<out TContent> where TContent : IContent
    {
        TContent CurrentContent { get; }
        HomePage StartPage { get; }
        HtmlString SchemaMarkup { get; }
    }
}