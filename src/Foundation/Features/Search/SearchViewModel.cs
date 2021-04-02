using EPiServer.Core;
using EPiServer.Find.Statistics.Api;
using EPiServer.Personalization.Commerce.Tracking;
using Foundation.Features.CatalogContent;
using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.Search
{
    public class SearchViewModel<T> : ContentViewModel<T> where T : IContent
    {
        public SearchViewModel()
        {
        }

        public SearchViewModel(T currentContent) : base(currentContent)
        {
        }

        public FilterOptionViewModel FilterOption { get; set; }
        public bool HasError { get; set; }
        public string ErrorMessage { get; set; }
        public DidYouMeanResult DidYouMeans { get; set; }
        public string Query { get; set; }
        public bool IsMobile { get; set; }
        public string RedirectUrl { get; set; }
        public ContentSearchViewModel ContentSearchResult { get; set; }
        public ContentSearchViewModel PdfSearchResult { get; set; }
        public IEnumerable<ProductTileViewModel> ProductViewModels { get; set; }
        public CategoriesFilterViewModel CategoriesFilter { get; set; }
        public List<KeyValuePair<string, string>> BreadCrumb { get; set; }
        public bool ShowProductSearchResults { get; set; }
        public bool ShowContentSearchResults { get; set; }
        public bool ShowPdfSearchResults { get; set; }
        public IEnumerable<Recommendation> Recommendations { get; set; }
    }
}
