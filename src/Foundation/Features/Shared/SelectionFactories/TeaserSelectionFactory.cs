using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Shared.SelectionFactories
{
    public class TeaserColorThemeSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Light", Value = "Light" },
                new SelectItem { Text = "Dark", Value = "Dark" }
            };
        }
    }

    public class TeaserTextAlignmentSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Left", Value = "Left" },
                new SelectItem { Text = "Right", Value = "Right" },
                new SelectItem { Text = "Center", Value = "Center" },
            };
        }
    }
}