using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.TeaserBlock
{
    public class TeaserBlockElementAlignmentSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Center", Value = "center" },
                new SelectItem { Text = "Left", Value = "left" },
                new SelectItem { Text = "Right", Value = "right" }
            };
        }
    }

    public class TeaserBlockHeadingStyleSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "None", Value = "none" },
                new SelectItem { Text = "Underline", Value = "underline" },
                new SelectItem { Text = "Overline", Value = "overline" },
                new SelectItem { Text = "Line through", Value = "line-through" },
            };
        }
    }

    public class TeaserBlockHeightStyleSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Small", Value = "260" },
                new SelectItem { Text = "Medium", Value = "400" },
                new SelectItem { Text = "Tall", Value = "550" },
            };
        }
    }

    public class TeaserBlockTextColorSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Black", Value = "black" },
                new SelectItem { Text = "White", Value = "white" },
                new SelectItem { Text = "Green Dark", Value = "#27747E" },
                new SelectItem { Text = "Off White", Value = "#E6F3EF" },
                new SelectItem { Text = "Yellow", Value = "#fec84d" }
            };
        }
    }
}