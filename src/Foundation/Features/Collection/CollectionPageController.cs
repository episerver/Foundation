using EPiServer.Web.Mvc;
using Foundation.Features.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.Collection
{
    public class CollectionPageController : PageController<CollectionPage>
    {
        public ActionResult Index(CollectionPage currentPage) => View(ContentViewModel.Create(currentPage));
    }
}