using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Media
{
    internal class ImageMediaDataAlignmentSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Left", Value = "justify-content: flex-start" },
                new SelectItem { Text = "Center", Value = "justify-content: center" },
                new SelectItem { Text = "Right", Value = "justify-content: flex-end" },
            };
        }
    }
}