using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.ProductHeroBlock
{
    public class ProductHeroBlockLayoutSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Callout on the left - image on the right", Value = "CalloutLeft" },
                new SelectItem { Text = "Callout on the right - image on the left", Value = "CalloutRight" }
            };
        }
    }

    public class ProductHeroBlockImagePositionSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Left", Value = "ImageLeft" },
                new SelectItem { Text = "Center", Value = "ImageCenter" },
                new SelectItem { Text = "Right", Value = "ImageRight" },
                new SelectItem { Text = "Use Paddings", Value = "ImagePaddings" }
            };
        }
    }
}
