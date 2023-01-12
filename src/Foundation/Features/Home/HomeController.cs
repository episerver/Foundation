namespace Foundation.Features.Home
{
    public class HomeController : PageController<HomePage>
    {
        public ActionResult Index(HomePage currentContent) => View(ContentViewModel.Create<HomePage>(currentContent));
    }
}