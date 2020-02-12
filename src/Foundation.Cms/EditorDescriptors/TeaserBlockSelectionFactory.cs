﻿using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Cms.EditorDescriptors
{
    class TeaserBlockElementOrderSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Image - Heading - Description - Text", Value = "ImageHeadingDescriptionText" },
                new SelectItem { Text = "Image - Description - Heading - Text", Value = "ImageDescriptionHeadingText" },
                new SelectItem { Text = "Heading - Image - Description - Text", Value = "HeadingImageDescriptionText" },
                new SelectItem { Text = "Heading - Description - Image - Text", Value = "HeadingDescriptionImageText" },
                new SelectItem { Text = "Description - Image - Heading - Text", Value = "DescriptionImageHeadingText" },
                new SelectItem { Text = "Description - Heading - Image - Text", Value = "DescriptionHeadingImageText" }
            };
        }
    }

    class TeaserBlockElementAlignmentSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Center", Value = "text-align: center" },
                new SelectItem { Text = "Left", Value = "text-align: left" },
                new SelectItem { Text = "Right", Value = "text-align: right" }
            };
        }
    }

    class TeaserBlockHeadingStyleSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "None", Value = "text-decoration: none" },
                new SelectItem { Text = "Underline", Value = "text-decoration: underline" },
                new SelectItem { Text = "Overline", Value = "text-decoration: overline" },
                new SelectItem { Text = "Line through", Value = "text-decoration: line-through" },
            };
        }
    }

    class TeaserBlockBackgroundColorSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "None", Value = "background-color: transparent" },
                new SelectItem { Text = "Black", Value = "background-color: black" },
                new SelectItem { Text = "Grey", Value = "background-color: grey" },
                new SelectItem { Text = "Beige", Value = "background-color: beige" },
                new SelectItem { Text = "Light Blue", Value = "background-color: #0081b2" },
                new SelectItem { Text = "Yellow", Value = "background-color: #fec84d" }
            };
        }
    }

    class TeaserBlockTextColorSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Text = "Black", Value = "color: black" },
                new SelectItem { Text = "White", Value = "color: white" },
                new SelectItem { Text = "Yellow", Value = "color: #fec84d" }
            };
        }
    }
}