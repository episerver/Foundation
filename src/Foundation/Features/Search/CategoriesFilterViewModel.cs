using System.Collections.Generic;

namespace Foundation.Features.Search
{
    public class CategoriesFilterViewModel
    {
        public CategoriesFilterViewModel()
        {
            Categories = new List<CategoryFilter>();
        }

        public IList<CategoryFilter> Categories { get; set; }
    }

    public class CategoryFilter
    {
        public CategoryFilter()
        {
            Children = new List<CategoryFilter>();
        }

        public string Url { get; set; }
        public string DisplayName { get; set; }
        public bool IsActive { get; set; }
        public bool IsBestBet { get; set; }
        public IList<CategoryFilter> Children { get; set; }
    }
}
