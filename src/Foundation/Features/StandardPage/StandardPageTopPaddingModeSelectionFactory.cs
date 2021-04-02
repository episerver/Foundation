using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.StandardPage
{
    public class StandardPageTopPaddingModeSelectionFactory : ISelectionFactory
    {
        public static class TopPaddingModes
        {
            public const string None = "None";
            public const string Half = "Half";
            public const string Full = "Full";
        }

        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "None", Value = TopPaddingModes.None },
                new SelectItem { Text = "Half", Value = TopPaddingModes.Half },
                new SelectItem { Text = "Full", Value = TopPaddingModes.Full },
            };
        }
    }
}