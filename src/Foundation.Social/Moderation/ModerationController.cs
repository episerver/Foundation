using EPiServer.Shell.Navigation;
using Foundation.Social.Services;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Social.Moderation
{
    public class ModerationController : Controller
    {
        private readonly ICommentManagerService _commentManagerService;

        public ModerationController(ICommentManagerService commentManagerService) => _commentManagerService = commentManagerService;

        [MenuItem("/global/foundation/commentsmanager", TextResourceKey = "/Shared/CommentsManager", SortIndex = 400)]

        [HttpGet]
        public ActionResult Index()
        {
            var model = new ModerationViewModel
            {
                Comments = _commentManagerService.Get(1, 100, out var total).ToList(),
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult Approve(string id)
        {
            _commentManagerService.Approve(id);

            return new ContentResult
            {
                Content = "Approve successfully.",
            };
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            _commentManagerService.Delete(id);

            return new ContentResult
            {
                Content = "Delete successfully.",
            };
        }
    }
}
