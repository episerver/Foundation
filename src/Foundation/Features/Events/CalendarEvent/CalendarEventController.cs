using EPiServer.Tracking.PageView;
using EPiServer.Web.Mvc;
using Foundation.Cms.Pages;
using Foundation.Cms.ViewModels;
using System.Web.Mvc;

namespace Foundation.Features.Events.CalendarEvent
{
    public class CalendarEventController : PageController<CalendarEventPage>
    {
        [PageViewTracking]
        public ActionResult Index(CalendarEventPage currentPage)
        {
            return View(ContentViewModel.Create(currentPage));
        }
    }
}