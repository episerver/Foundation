namespace Foundation.Features.Shared.SelectionFactories
{
    public static class BootstrapCardRatioSelections
    {
        public const string OneOne = "ratio-1x1";
        public const string FourThree = "ratio-4x3";
        public const string SixteenNine = "ratio-16x9";
        public const string TwentyOneNine = "ratio-21x9";
    }

    public class BootstrapCardRatioSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Value = BootstrapCardRatioSelections.FourThree, Text = "4x3"},
                new SelectItem { Value = BootstrapCardRatioSelections.OneOne, Text = "1x1"},
                new SelectItem { Value = BootstrapCardRatioSelections.SixteenNine, Text = "16x9"},
                new SelectItem { Value = BootstrapCardRatioSelections.TwentyOneNine, Text = "21x9"},
            };
        }
    }
}
