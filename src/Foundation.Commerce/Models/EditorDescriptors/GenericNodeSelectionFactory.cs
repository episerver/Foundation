using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Commerce.Models.EditorDescriptors
{
    public class GenericNodeSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new List<ISelectItem>
            {
                new SelectItem { Text = "List", Value = "List" },
                new SelectItem { Text = "Grid", Value = "Grid" }
            };
        }
    }
}
