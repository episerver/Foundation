namespace Foundation.Features.LandingPages.TwoColumnLandingPage
{
    public class TwoColumnLandingPageController : PageController<TwoColumnLandingPage>
    {
        public ActionResult Index(TwoColumnLandingPage currentPage)
        {
            var model = ContentViewModel.Create(currentPage);
            return View(model);
        }
    }
}