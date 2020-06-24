using EPiServer.Tracking.PageView;
using EPiServer.Web.Mvc;
using System;
using System.Web.Mvc;

namespace Foundation.Features.MyAccount.Bookmarks
{
    /// <summary>
    /// A page to list all bookmarks belonging to a customer
    /// </summary>
    public class BookmarksController : PageController<BookmarksPage>
    {
        private readonly IBookmarksService _bookmarksService;

        public BookmarksController(IBookmarksService bookmarksService)
        {
            _bookmarksService = bookmarksService;
        }

        [PageViewTracking]
        public ActionResult Index(BookmarksPage currentPage)
        {
            var model = new BookmarksViewModel(currentPage)
            {
                Bookmarks = _bookmarksService.Get(),
                CurrentContent = currentPage
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Bookmark(Guid contentId)
        {
            _bookmarksService.Add(contentId);
            return Json(new { Success = true });
        }

        [HttpPost]
        public ActionResult Unbookmark(Guid contentId)
        {
            _bookmarksService.Remove(contentId);
            return Json(new { Success = true });
        }

        [HttpPost]
        public ActionResult Remove(Guid contentGuid)
        {
            _bookmarksService.Remove(contentGuid);
            return Json(new { Success = true });
        }
    }
}