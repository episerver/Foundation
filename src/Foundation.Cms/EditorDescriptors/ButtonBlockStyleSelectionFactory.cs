using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Cms.EditorDescriptors
{
    public class ButtonBlockStyleSelectionFactory : ISelectionFactory
    {
        public static class ButtonBlockStyles
        {
            public const string TransparentBlack = "button-transparent-black";
            public const string TransparentWhite = "button-transparent-white";
            public const string Dark = "button-black";
            public const string White = "button-white";
            public const string YellowBlack = "button-yellow-black";
            public const string YellowWhite = "button-yellow-white";
        }

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var dic = new Dictionary<string, string>
            {
                { "Transparent Black", ButtonBlockStyles.TransparentBlack },
                { "Transparent White", ButtonBlockStyles.TransparentWhite },
                { "Dark", ButtonBlockStyles.Dark },
                { "White", ButtonBlockStyles.White },
                { "Yellow Black", ButtonBlockStyles.YellowBlack },
                { "Yellow White", ButtonBlockStyles.YellowWhite },
            };

            return dic.Select(x => new SelectItem { Text = x.Key, Value = x.Value });
        }
    }
}
