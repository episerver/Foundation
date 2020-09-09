using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.ButtonBlock
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

    public class ButtonBlockStyleSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new List<SelectItem>
            {
                new SelectItem { Text = "Transparent Black", Value = ButtonBlockStyles.TransparentBlack },
                new SelectItem { Text = "Transparent White", Value = ButtonBlockStyles.TransparentWhite },
                new SelectItem { Text = "Dark", Value = ButtonBlockStyles.Dark },
                new SelectItem { Text = "White", Value = ButtonBlockStyles.White },
                new SelectItem { Text = "Yellow Black", Value = ButtonBlockStyles.YellowBlack },
                new SelectItem { Text = "Yellow White", Value = ButtonBlockStyles.YellowWhite }
            };
        }
    }
}