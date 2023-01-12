namespace Foundation.Features.Collection
{
    public class CollectionPageController : PageController<CollectionPage>
    {
        public ActionResult Index(CollectionPage currentPage) => View(ContentViewModel.Create(currentPage));
    }
}