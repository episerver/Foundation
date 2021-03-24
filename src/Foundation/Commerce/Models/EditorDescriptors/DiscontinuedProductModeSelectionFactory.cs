using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Commerce.Models.EditorDescriptors
{
    public static class DiscontinuedProductMode
    {
        public const string None = "None";
        public const string Hide = "Hide";
        public const string DemoteToBottom = "Demote to bottom";
    }

    public class DiscontinuedProductModeSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem{ Text = "None", Value = DiscontinuedProductMode.None },
                new SelectItem{ Text = "Hide", Value = DiscontinuedProductMode.Hide },
                new SelectItem{ Text = "Demote to bottom", Value = DiscontinuedProductMode.DemoteToBottom }
            };
        }
    }
}