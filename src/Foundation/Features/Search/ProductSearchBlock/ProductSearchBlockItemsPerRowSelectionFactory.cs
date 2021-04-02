using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Search.ProductSearchBlock
{
    public class ProductSearchBlockItemsPerRowSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "3", Value = 3 },
                new SelectItem { Text = "4", Value = 4 },
                new SelectItem { Text = "6", Value = 6 }
            };
        }
    }
}
