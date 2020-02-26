using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Cms.EditorDescriptors
{
    class FoundationBlockDataPaddingUnitSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "%", Value = "%" },
                new SelectItem { Text = "px", Value = "px" }
            };
        }
    }
}
