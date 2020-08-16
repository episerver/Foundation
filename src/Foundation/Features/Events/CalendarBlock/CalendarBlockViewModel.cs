using EPiServer.Core;

namespace Foundation.Features.Events.CalendarBlock
{
    public class CalendarBlockViewModel
    {
        public CalendarBlockViewModel(CalendarBlock block)
        {
            ViewMode = block.ViewMode;
            BlockId = ((IContent)block).ContentLink.ID;
            CurrentBlock = block;
        }

        public string ViewMode { get; set; }
        public int BlockId { get; set; }
        public CalendarBlock CurrentBlock { get; set; }
    }
}
