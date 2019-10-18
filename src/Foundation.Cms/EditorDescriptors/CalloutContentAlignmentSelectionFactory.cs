using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Cms.EditorDescriptors
{
    public class CalloutContentAlignmentSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var dic = new Dictionary<string, string>
            {
                {"Left", CalloutContentAlignments.Left},
                {"Right", CalloutContentAlignments.Right},
                {"Center", CalloutContentAlignments.Center}
            };

            return dic.Select(x => new SelectItem { Text = x.Key, Value = x.Value });
        }
    }
}