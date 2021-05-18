using EPiServer.Core;
using Foundation.Features.Home;
using Microsoft.AspNetCore.Html;

namespace Foundation.Features.Shared
{
    public interface IContentViewModel<out TContent> where TContent : IContent
    {
        TContent CurrentContent { get; }
        HomePage StartPage { get; }
        HtmlString SchemaMarkup { get; }
    }
}