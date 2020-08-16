using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Events.CalendarBlock
{
    public class CalendarViewModeSelectionFactory : ISelectionFactory
    {
        public static class CalendarViewModes
        {
            public const string Day = "dayGridDay";
            public const string Week = "dayGridWeek";
            public const string Month = "dayGridMonth";
            public const string Upcoming = "listMonth";
        }

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Day", Value = CalendarViewModes.Day },
                new SelectItem { Text = "Week", Value = CalendarViewModes.Week},
                new SelectItem { Text = "Month", Value = CalendarViewModes.Month },
                new SelectItem { Text = "Upcoming", Value = CalendarViewModes.Upcoming }
            };
        }
    }
}