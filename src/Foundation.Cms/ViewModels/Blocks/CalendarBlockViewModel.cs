using Foundation.Cms.Blocks;
using Foundation.Cms.Pages;
using System.Collections.Generic;

namespace Foundation.Cms.ViewModels.Blocks
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
