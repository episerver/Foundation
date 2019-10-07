using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Cms.EditorDescriptors
{
    class TeaserBlockTextPlacementSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem {Text = "Above image", Value = "AboveImage"},
                new SelectItem {Text = "Below image", Value = "BelowImage"}
            };
        }
    }
}