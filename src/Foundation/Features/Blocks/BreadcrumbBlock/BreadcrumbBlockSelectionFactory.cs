using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.BreadcrumbBlock
{
    public class BreadcrumbSeparatorSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new List<SelectItem>
            {
                new SelectItem {Text = "> Single arrow", Value = ">"},
                new SelectItem {Text = "/ Forward slash", Value = "/"},
                new SelectItem {Text = @"\ Backward slash", Value = @"\"},
                new SelectItem {Text = "» Double arrow", Value = "»"},
                new SelectItem {Text = "| Pipe", Value = "|"},
                new SelectItem {Text = ": Pipe", Value = ":"}
            };
        }
    }

    public class BreadcrumbAlignmentOptionSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new List<SelectItem>
            {
                new SelectItem { Text = "Left", Value = "flex-start" },
                new SelectItem { Text = "Right", Value = "flex-end" },
                new SelectItem { Text = "Center", Value = "flex-center" }
            };
        }
    }
}
