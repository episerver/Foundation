using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Commerce.Models.EditorDescriptors
{
    public class ProductStatusSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Active", Value = "Active" },
                new SelectItem { Text = "Inactive", Value = "Inactive" },
                new SelectItem { Text = "Discontinued", Value = "Discontinued" }
            };
        }
    }
}