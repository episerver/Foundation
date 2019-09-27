using EPiServer.Core;

namespace Foundation.Find.Cms.ViewModels
{
    public interface ISearchViewModelFactory
    {
        TModel Create<TModel, TContent>(TContent currentContent, IArgs args)
            where TContent : IContent
            where TModel : CmsSearchViewModel<TContent>, new();
    }
}
