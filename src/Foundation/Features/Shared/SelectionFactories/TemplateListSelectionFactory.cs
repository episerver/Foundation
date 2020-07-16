using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Features.Shared.SelectionFactories
{
    public static class TemplateSelections
    {
        public const string Grid = "Grid";
        public const string ImageLeft = "Left";
        public const string ImageTop = "Top";
        public const string NoImage = "NoImage";
        public const string Highlight = "Highlight";
        public const string Card = "Card";
        public const string Insight = "Insight";
    }

    public class TemplateListSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Value = TemplateSelections.Grid, Text = "Grid"},
                new SelectItem { Value = TemplateSelections.ImageLeft, Text = "Image on the left"},
                new SelectItem { Value = TemplateSelections.ImageTop, Text = "Image on the top"},
                new SelectItem { Value = TemplateSelections.NoImage, Text = "No image"},
                new SelectItem { Value = TemplateSelections.Highlight, Text = "Highlight panel"},
                new SelectItem { Value = TemplateSelections.Card, Text = "Card"},
                new SelectItem { Value = TemplateSelections.Insight, Text = "Insight"},
            };
        }
    }
}
