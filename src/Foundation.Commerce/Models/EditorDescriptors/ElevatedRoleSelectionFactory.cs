using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Models.EditorDescriptors
{
    public class ElevatedRoleSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var dic = new Dictionary<string, string>()
            {
                {"None", "None"},
                {"Reader", "Reader"}
            };

            return dic.Select(x => new SelectItem() { Text = x.Key, Value = x.Value });
        }
    }
}