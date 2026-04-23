namespace Foundation.Features.MyAccount.Bookmarks
{
    [ApiController]
    [Route("[controller]")]
    public class BookmarksApiController : ControllerBase
    {
        private readonly IBookmarksService _bookmarksService;

        public BookmarksApiController(IBookmarksService bookmarksService)
        {
            _bookmarksService = bookmarksService;
        }

        [HttpPost]
        [Route("Bookmark")]
        public ActionResult Bookmark(Guid contentId)
        {
            _bookmarksService.Add(contentId);
            return Ok(new { Success = true });
        }

        [HttpPost]
        [Route("Unbookmark")]
        public ActionResult Unbookmark(Guid contentId)
        {
            _bookmarksService.Remove(contentId);
            return Ok(new { Success = true });
        }

        [HttpPost]
        [Route("Remove")]
        public ActionResult Remove(Guid contentGuid)
        {
            _bookmarksService.Remove(contentGuid);
            return Ok(new { Success = true });
        }
    }
}