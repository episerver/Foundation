using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.IframeBlock
{
    public class RatioSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "1x1", Value = "ratio-1x1" },
                new SelectItem { Text = "4x3", Value = "ratio-4x3" },
                new SelectItem { Text = "16x9", Value = "ratio-16x9" },
                new SelectItem { Text = "21x9", Value = "ratio-21x9" },
            };
        }
    }
}
