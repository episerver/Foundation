using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Cms.EditorDescriptors
{
    public class BreadcrumbAlignmentOptionSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new List<SelectItem>
            {
                new SelectItem { Text = "Left", Value = "flex-start" },
                new SelectItem { Text = "Right", Value = "flex-end" },
                new SelectItem { Text = "Center", Value = "flex-center" }
            };
        }
    }
}
