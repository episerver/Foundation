using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Shared.SelectionFactories
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
            return new ISelectItem[]
            {
                new SelectItem { Text = "Beginning", Value = InclusionOrdering.Beginning },
                new SelectItem { Text = "End", Value = InclusionOrdering.End },
                new SelectItem { Text = "Random", Value = InclusionOrdering.Random }
            };
        }
    }
}