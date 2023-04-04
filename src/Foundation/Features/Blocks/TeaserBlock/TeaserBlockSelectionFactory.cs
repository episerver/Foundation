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

    public static class ButtonBlockStyles
    {
        public const string UseGlobal = "button-use-global";
        public const string SetManually = "button-set-manually";
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
                new SelectItem { Text = "Use Global", Value = ButtonBlockStyles.UseGlobal },
                new SelectItem { Text = "Set Manually", Value = ButtonBlockStyles.SetManually },
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