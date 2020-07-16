using System;
using System.Collections.Generic;

namespace Foundation.Cms
{
    public class PagingInfo
    {
        public PagingInfo()
        {

        }

        public PagingInfo(int pageId, int pageSize, int pageIndex)
        {
            PageId = pageId;
            PageSize = pageSize;
            PageNumber = pageIndex;
        }

        public int PageSize { get; set; } = 5;
        public int PageNumber { get; set; } = 1;
        public int TotalRecord { get; set; }
        public int PageId { get; set; }

        public int PageCount => (PageSize == -1 && TotalRecord > 0) ? 1 : (int)Math.Ceiling((double)TotalRecord / PageSize);

        public List<int> Pages
        {
            get
            {
                if (TotalRecord == 0)
                {
                    return new List<int>();
                }

                var totalPages = (TotalRecord + PageSize - 1) / PageSize;
                var pages = new List<int>();
                var startPage = PageNumber > 2 ? PageNumber - 2 : 1;
                for (var page = startPage;
                    page < Math.Min(totalPages >= 5 ? startPage + 5 : startPage + totalPages, totalPages + 1);
                    page++)
                {
                    pages.Add(page);
                }

                return pages;
            }
        }
    }
}