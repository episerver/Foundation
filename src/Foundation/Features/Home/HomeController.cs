using EPiServer.Web.Mvc;
using Foundation.Features.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.Home
{
    public class HomeController : PageController<HomePage>
    {
        public ActionResult Index(HomePage currentContent) => View(ContentViewModel.Create<HomePage>(currentContent));
    }
}