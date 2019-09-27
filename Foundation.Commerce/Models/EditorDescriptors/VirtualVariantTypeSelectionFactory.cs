using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Models.EditorDescriptors
{
    public class VirtualVariantTypeSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var dic = new Dictionary<string, string>
            {
                {"None", "None"},
                {"Key", "Key"},
                {"File", "File"},
                {"Elevated Role", "ElevatedRole"}
            };

            return dic.Select(x => new SelectItem { Text = x.Key, Value = x.Value });
        }
    }
}
