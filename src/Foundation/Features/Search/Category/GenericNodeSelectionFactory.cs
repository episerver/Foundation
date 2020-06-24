using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Search.Category
{
    public class GenericNodeSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "List", Value = "List" },
                new SelectItem { Text = "Grid", Value = "Grid" }
            };
        }
    }
}
