using EPiServer.Web.Mvc;
using Foundation.Cms.Pages;
using Foundation.Cms.Personalization;
using Foundation.Cms.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Foundation.Features.Events.CalendarEvent
{
    public class CalendarEventController : PageController<CalendarEventPage>
    {
        private readonly ICmsTrackingService _trackingService;

        public CalendarEventController(ICmsTrackingService trackingService)
        {
            _trackingService = trackingService;
        }

        public async Task<ActionResult> Index(CalendarEventPage currentPage)
        {
            await _trackingService.PageViewed(HttpContext, currentPage);
            return View(ContentViewModel.Create(currentPage));
        }
    }
}