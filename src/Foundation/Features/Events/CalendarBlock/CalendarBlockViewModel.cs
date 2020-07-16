using Foundation.Features.Events.CalendarEvent;
using System.Collections.Generic;

namespace Foundation.Features.Events.CalendarBlock
{
    public class CalendarBlockViewModel
    {
        public CalendarBlockViewModel(CalendarBlock block)
        {
            ViewMode = block.ViewMode;
            CurrentBlock = block;
        }

        public string ViewMode { get; set; }
        public IEnumerable<CalendarEventPage> Events { get; set; }
        public CalendarBlock CurrentBlock { get; set; }
    }
}
