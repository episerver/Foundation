namespace Foundation.Features.LandingPages.LandingPage
{
    public class LandingPageController : PageController<LandingPage>
    {
        public ActionResult Index(LandingPage currentPage)
        {
            var model = ContentViewModel.Create(currentPage);
            return View(model);
        }
    }
}