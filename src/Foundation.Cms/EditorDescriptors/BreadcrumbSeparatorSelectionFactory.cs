using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Cms.EditorDescriptors
{
    public class BreadcrumbSeparatorSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var separators = new List<SelectItem>
            {
                new SelectItem {Text = "> Single arrow", Value = ">"},
                new SelectItem {Text = "/ Forward slash", Value = "/"},
                new SelectItem {Text = @"\ Backward slash", Value = @"\"},
                new SelectItem {Text = "» Double arrow", Value = "»"},
                new SelectItem {Text = "| Pipe", Value = "|"},
                new SelectItem {Text = ": Pipe", Value = ":"}
            };

            return separators;
        }
    }
}
