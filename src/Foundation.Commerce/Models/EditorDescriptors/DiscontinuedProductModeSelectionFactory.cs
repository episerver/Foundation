using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Models.EditorDescriptors
{
    public class DiscontinuedProductModeSelectionFactory : ISelectionFactory
    {
        public static class DiscontinuedProductMode
        {
            public const string None = "None";
            public const string Hide = "Hide";
            public const string DemoteToBottom = "Demote to bottom";
        }

        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var dic = new Dictionary<string, string>()
            {
                {"None", DiscontinuedProductMode.None},
                {"Hide", DiscontinuedProductMode.Hide},
                {"Demote to bottom", DiscontinuedProductMode.DemoteToBottom}
            };

            return dic.Select(x => new SelectItem() { Text = x.Key, Value = x.Value });
        }
    }
}