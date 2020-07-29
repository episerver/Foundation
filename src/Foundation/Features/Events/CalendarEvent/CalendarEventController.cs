using EPiServer.Tracking.PageView;
using EPiServer.Web.Mvc;
using Foundation.Features.Shared;
using System.Web.Mvc;

namespace Foundation.Features.Events.CalendarEvent
{
    public class CalendarEventController : PageController<CalendarEventPage>
    {
        [PageViewTracking]
        public ActionResult Index(CalendarEventPage currentPage) => View(ContentViewModel.Create(currentPage));
    }
}