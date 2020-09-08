using EPiServer.Core;
using Foundation.Features.Home;
using System.Web;

public interface IContentViewModel<out TContent> where TContent : IContent
{
    TContent CurrentContent { get; }
    HomePage StartPage { get; }
    HtmlString SchemaMarkup { get; }
}