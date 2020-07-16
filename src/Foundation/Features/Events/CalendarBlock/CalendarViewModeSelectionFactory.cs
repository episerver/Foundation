using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Events.CalendarBlock
{
    public class CalendarViewModeSelectionFactory : ISelectionFactory
    {
        public static class CalendarViewModes
        {
            public const string Day = "agendaDay";
            public const string Week = "agendaWeek";
            public const string Month = "month";
            public const string List = "List";
        }

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Day", Value = CalendarViewModes.Day },
                new SelectItem { Text = "Week", Value = CalendarViewModes.Week},
                new SelectItem { Text = "Month", Value = CalendarViewModes.Month },
                new SelectItem { Text = "List", Value = CalendarViewModes.List }
            };
        }
    }
}