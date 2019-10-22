using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Cms.EditorDescriptors
{
    public class TemplateBlogListSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Value = TemplateSelections.Grid, Text = "Grid"},
                new SelectItem { Value = TemplateSelections.ImageLeft, Text = "Image on the left"},
                new SelectItem { Value = TemplateSelections.ImageTop, Text = "Image on the top"},
            };
        }
    }

    public class TemplateSelections
    {
        public const string Grid = "Grid";
        public const string ImageLeft = "Left";
        public const string ImageTop = "Top";
    }
}
