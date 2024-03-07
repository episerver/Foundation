using Foundation.Social.Services;

namespace Foundation.Social.Moderation
{
    public class ModerationController : Controller
    {
        private readonly ICommentManagerService _commentManagerService;

        public ModerationController(ICommentManagerService commentManagerService) => _commentManagerService = commentManagerService;

        [Route("episerver/foundation/moderation")]
        [HttpGet]
        public ActionResult Index()
        {
            var model = new ModerationViewModel
            {
                Comments = _commentManagerService.Get(1, 100, out var total).ToList(),
            };

            return View("/Infrastructure/Social/Views/Moderation/Index.cshtml", model);
        }

        [HttpPost]
        [Route("episerver/foundation/moderation/approve")]
        public ActionResult Approve(string id)
        {
            _commentManagerService.Approve(id);

            return new ContentResult
            {
                Content = "Approve successfully.",
            };
        }

        [HttpPost]
        [Route("episerver/foundation/moderation/delete")]
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
