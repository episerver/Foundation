using EPiServer.Web.Mvc;
using Foundation.Features.Shared;
using System.Web.Mvc;

namespace Foundation.Features.Collection
{
    public class CollectionPageController : PageController<CollectionPage>
    {
        public ActionResult Index(CollectionPage currentPage)
        {
            return View(ContentViewModel.Create(currentPage));
        }
    }
}