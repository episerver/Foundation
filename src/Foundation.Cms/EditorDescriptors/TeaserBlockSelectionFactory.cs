using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Cms.EditorDescriptors
{
    class TeaserBlockElementOrderSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem {Text = "Image - Heading - Text", Value = "ImageHeadingText"},
                new SelectItem {Text = "Image - Text - Heading", Value = "ImageTextHeading"},
                new SelectItem {Text = "Heading - Image - Text", Value = "HeadingImageText"},
                new SelectItem {Text = "Heading - Text - Image", Value = "HeadingTextImage"},
                new SelectItem {Text = "Text - Image - Heading", Value = "TextImageHeading"},
                new SelectItem {Text = "Text - Heading - Image", Value = "TextHeadingImage"}
            };
        }
    }

    class TeaserBlockElementAlignmentSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem {Text = "Elements align center", Value = "ElementCenter"},
                new SelectItem {Text = "Elements align left", Value = "ElementLeft"},
                new SelectItem {Text = "Elements align right", Value = "ElementRight"}
            };
        }
    }
}