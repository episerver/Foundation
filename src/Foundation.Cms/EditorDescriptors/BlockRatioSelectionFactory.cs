using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Cms.EditorDescriptors
{
    public class BlockRatioSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "10-2", Value = "10-2" },
                new SelectItem { Text = "10-3", Value = "10-3" },
                new SelectItem { Text = "10-5", Value = "10-5" },
                new SelectItem { Text = "10-10", Value = "10-10" },
                new SelectItem { Text = "10-12.5", Value = "10-12.5" },
                new SelectItem { Text = "10-15", Value = "10-15" },
                new SelectItem { Text = "10-17.5", Value = "10-17.5" }
            };
        }
    }
}
