using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.CallToActionBlock
{
    public class BackgroundImageSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new List<SelectItem>
            {
                new SelectItem { Text = "Fit width", Value = "image-fit-width" },
                new SelectItem { Text = "Fit height", Value = "image-fit-height" },
                new SelectItem { Text = "Stretch", Value = "image-stretch" },
                new SelectItem { Text = "Tile", Value = "image-tile" },
                new SelectItem { Text = "Default", Value = "image-default" }
            };
        }
    }
}
