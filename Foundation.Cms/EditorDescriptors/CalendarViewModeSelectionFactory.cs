using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Cms.EditorDescriptors
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
            var dic = new Dictionary<string, string>
            {
                {"Day", CalendarViewModes.Day},
                {"Week", CalendarViewModes.Week},
                {"Month", CalendarViewModes.Month},
                {"List", CalendarViewModes.List}
            };

            return dic.Select(x => new SelectItem { Text = x.Key, Value = x.Value });
        }
    }
}