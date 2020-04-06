using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Cms.EditorDescriptors
{
    public class CalloutContentAlignmentSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Left", Value = "left" },
                new SelectItem { Text = "Right", Value = "right" },
                new SelectItem { Text = "Center", Value = "center" },
            };
        }
    }

    public class CalloutPositionSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Top", Value = "flex-start" },
                new SelectItem { Text = "Middle", Value = "center" },
                new SelectItem { Text = "Bottom", Value = "flex-end" },
            };
        }
    }
}
