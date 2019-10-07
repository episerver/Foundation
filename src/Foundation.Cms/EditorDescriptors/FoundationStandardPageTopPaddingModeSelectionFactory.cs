using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Cms.EditorDescriptors
{
    public class FoundationStandardPageTopPaddingModeSelectionFactory : ISelectionFactory
    {
        public static class FoundationStandardPageTopPaddingModes
        {
            public const string None = "None";
            public const string Half = "Half";
            public const string Full = "Full";
        }
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var dic = new Dictionary<string, string>
            {
                { "None", FoundationStandardPageTopPaddingModes.None },
                { "Half", FoundationStandardPageTopPaddingModes.Half },
                { "Full", FoundationStandardPageTopPaddingModes.Full },
            };

            return dic.Select(x => new SelectItem { Text = x.Key, Value = x.Value });
        }
    }
}