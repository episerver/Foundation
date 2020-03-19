using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Foundation.Find.Cms.ViewModels
{
    public class CmsFilterOptionViewModel
    {
        public string SelectedFacet { get; set; }
        public IEnumerable<SelectListItem> Sorting { get; set; }
        public string Sort { get; set; } = "Position";
        public string SortDirection { get; set; } = "Asc";
        public int Page { get; set; }
        public string Q { get; set; }
        public int TotalCount { get; set; }
        public int PageSize { get; set; } = 15;
        public string ViewSwitcher { get; set; }
        public decimal Confidence { get; set; }
        public bool HighlightTitle { get; set; }
        public bool HighlightExcerpt { get; set; }
        public string SectionFilter { get; set; }
        public bool SearchContent { get; set; }
        public bool IncludeImagesContent { get; set; }
        public bool TrackData { get; set; } = true;

        public List<int> Pages
        {
            get
            {
                if (TotalCount == 0)
                {
                    return new List<int>();
                }
                var totalPages = (TotalCount + PageSize - 1) / PageSize;
                var pages = new List<int>();
                var startPage = Page > 2 ? Page - 2 : 1;
                for (var page = startPage; page < Math.Min((totalPages >= 5 ? startPage + 5 : startPage + totalPages), totalPages + 1); page++)
                {
                    pages.Add(page);
                }
                return pages;
            }
        }

    }
}
