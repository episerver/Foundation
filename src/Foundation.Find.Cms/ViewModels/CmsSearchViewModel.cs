using EPiServer.Core;
using EPiServer.Find.Statistics.Api;
using Foundation.Cms.ViewModels;

namespace Foundation.Find.Cms.ViewModels
{
    public class CmsSearchViewModel<T> : ContentViewModel<T> where T : IContent
    {
        public CmsFilterOptionViewModel FilterOption { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public DidYouMeanResult DidYouMeans { get; set; }
        public string Query { get; set; }
        public bool IsMobile { get; set; }
        public string RedirectUrl { get; set; }
        public ContentSearchViewModel ContentSearchResult { get; set; }

        public CmsSearchViewModel()
        {

        }

        public CmsSearchViewModel(T currentContent) : base(currentContent)
        {

        }
    }
}
