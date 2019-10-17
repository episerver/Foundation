using EPiServer.Core;
using Foundation.Cms.Categories;
using Foundation.Cms.Pages;
using Foundation.Cms.ViewModels.Categories;
using System.Collections.Generic;

namespace Foundation.Find.Cms.ViewModels
{
    public class CategorySearchViewModel : StandardCategoryViewModel
    {
        public CategorySearchViewModel() { }
        public CategorySearchViewModel(StandardCategory category) : base(category) { }

        public CategorySearchResults SearchResults { get; set; }
    }

    public class CategorySearchResults
    {
        public CategorySearchResults()
        {
            RelatedPages = new List<FoundationPageData>();
            Pagination = new Pagination();
        }

        public IEnumerable<FoundationPageData> RelatedPages { get; set; }
        public Pagination Pagination { get; set; }
    }

    public class Pagination
    {
        public Pagination()
        {
            Page = 1;
            PageSize = 15;
            Categories = new List<ContentReference>();
            Sort = CategorySorting.PublishedDate.ToString();
            SortDirection = "desc";
        }

        public int Page { get; set; }
        public int PageSize { get; set; }
        public int TotalPage { get; set; }
        public int TotalMatching { get; set; }
        public string Sort { get; set; }
        public string SortDirection { get; set; }
        public IEnumerable<ContentReference> Categories { get; set; }
    }

    public enum CategorySorting
    {
        PublishedDate,
        Name,
    }
}
