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
                new SelectItem { Text = "5:1", Value = "5:1" },
                new SelectItem { Text = "2:1", Value = "2:1" },
                new SelectItem { Text = "16:9", Value = "16:9" },
                new SelectItem { Text = "3:2", Value = "3:2" },
                new SelectItem { Text = "4:3", Value = "4:3" },
                new SelectItem { Text = "1:1", Value = "1:1" },
                new SelectItem { Text = "2:3", Value = "2:3" },
                new SelectItem { Text = "9:16", Value = "9:16" }
            };
        }
    }
}