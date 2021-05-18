using EPiServer.Web.Mvc;
using Foundation.Features.Shared;
using Microsoft.AspNetCore.Mvc;

namespace Foundation.Features.Events.CalendarEvent
{
    public class CalendarEventController : PageController<CalendarEventPage>
    {
        public ActionResult Index(CalendarEventPage currentPage) => View(ContentViewModel.Create(currentPage));
    }
}