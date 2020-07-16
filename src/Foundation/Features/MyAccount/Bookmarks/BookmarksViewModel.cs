using Foundation.Commerce.Customer;
using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.MyAccount.Bookmarks
{
    public class BookmarksViewModel : ContentViewModel<BookmarksPage>
    {
        public List<BookmarkModel> Bookmarks { get; set; }
        public BookmarksViewModel(BookmarksPage currentPage) : base(currentPage) { }
        public BookmarksViewModel() : base() { }
    }
}
