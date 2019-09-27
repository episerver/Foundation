using EPiServer.Core;
using Foundation.Cms.Blocks;
using System.Collections.Generic;

namespace Foundation.Cms.ViewModels.Blocks
{
    public class CalendarEventBlockViewModel
    {
        public CalendarEventBlockViewModel(CalendarEventBlock block) => ViewMode = block.ViewMode;
        public string ViewMode { get; set; }
        public IEnumerable<PageData> Events { get; set; }
    }
}
