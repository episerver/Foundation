﻿using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;

namespace Foundation.Cms.EditorDescriptors
{
    public class PreviewOptionSelectionFactory : ISelectionFactory
    {
        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            return new ISelectItem[]
            {
                new SelectItem { Value = PreviewOptions.OneThird, Text = "One third width" },
                new SelectItem { Value = PreviewOptions.OneThird, Text = "Half width" },
                new SelectItem { Value = PreviewOptions.OneThird, Text = "Full width" }
            };
        }
    }

    public class PreviewOptions
    {
        public const string OneThird = "1/3";
        public const string Half = "1/2";
        public const string Full = "1";
    }
}
