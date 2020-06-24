using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Shared.SelectionFactories
{
    public static class PreviewOptions
    {
        public const string OneThird = "1/3";
        public const string Half = "1/2";
        public const string Full = "1";
    }

    public class PreviewOptionSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Value = PreviewOptions.OneThird, Text = "One third width" },
                new SelectItem { Value = PreviewOptions.Half, Text = "Half width" },
                new SelectItem { Value = PreviewOptions.Full, Text = "Full width" }
            };
        }
    }
}
