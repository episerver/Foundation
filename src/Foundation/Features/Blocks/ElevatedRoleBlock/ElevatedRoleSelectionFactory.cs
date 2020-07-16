using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.ElevatedRoleBlock
{
    public class ElevatedRoleSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "None", Value = "None"},
                new SelectItem { Text = "Reader", Value = "Reader"}
            };
        }
    }
}