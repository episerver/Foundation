using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Models.EditorDescriptors
{
    public class InclusionOrderingSelectionFactory : ISelectionFactory
    {
        public static class InclusionOrdering
        {
            public const string Beginning = "Beginning";
            public const string End = "End";
            public const string Random = "Random";
        }

        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var dic = new Dictionary<string, string>()
            {
                {"Beginning", InclusionOrdering.Beginning},
                {"End", InclusionOrdering.End},
                {"Random", InclusionOrdering.Random}
            };

            return dic.Select(x => new SelectItem() { Text = x.Key, Value = x.Value });
        }
    }
}