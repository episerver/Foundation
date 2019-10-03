using EPiServer.Web.Mvc;
using Foundation.Cms.Pages;
using Foundation.Cms.Personalization;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Customer.ViewModels;
using System;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.MyAccount.Bookmarks
{
    /// <summary>
    /// A page to list all bookmarks belonging to a customer
    /// </summary>
    public class BookmarksController : PageController<BookmarksPage>
    {
        private readonly IBookmarksService _bookmarksService;
        private readonly ICmsTrackingService _trackingService;

        public BookmarksController(IBookmarksService bookmarksService,
            ICmsTrackingService trackingService)
        {
            _bookmarksService = bookmarksService;
            _trackingService = trackingService;
        }

        public async Task<ActionResult> Index(BookmarksPage currentPage)
        {
            await _trackingService.PageViewed(HttpContext, currentPage);
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