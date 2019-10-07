using Foundation.Cms.Pages;
using Foundation.Cms.ViewModels;
using System.Collections.Generic;

namespace Foundation.Commerce.Customer.ViewModels
{
    public class BookmarksViewModel : ContentViewModel<BookmarksPage>
    {
        public List<BookmarkModel> Bookmarks { get; set; }
        public BookmarksViewModel(BookmarksPage currentPage) : base(currentPage) { }
        public BookmarksViewModel() : base() { }
    }
}
