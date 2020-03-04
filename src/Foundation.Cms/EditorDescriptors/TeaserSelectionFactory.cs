using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Cms.EditorDescriptors
{
    public class TeaserColorThemeSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Light", Value = ColorThemes.Light },
                new SelectItem { Text = "Dark", Value = ColorThemes.Dark }
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