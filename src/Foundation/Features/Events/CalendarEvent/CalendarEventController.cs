namespace Foundation.Features.Events.CalendarEvent
{
    public class CalendarEventController : PageController<CalendarEventPage>
    {
        public ActionResult Index(CalendarEventPage currentPage) => View(ContentViewModel.Create(currentPage));
    }
}