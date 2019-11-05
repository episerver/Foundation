﻿using EPiServer.Core;
using Foundation.Cms.Blocks;
using System.Collections.Generic;

namespace Foundation.Cms.ViewModels.Blocks
{
    public class PageListBlockViewModel : BlockViewModel<PageListBlock>
    {
        public PageListBlockViewModel(PageListBlock block) : base(block)
        {
            Heading = block.Heading;
            ShowIntroduction = block.IncludeTeaserText;
            ShowPublishDate = block.IncludePublishDate;
            PaddingStyles = block.PaddingStyles;
        }

        public string Heading { get; set; }
        public IEnumerable<PageData> Pages { get; set; }
        public bool ShowIntroduction { get; set; }
        public bool ShowPublishDate { get; set; }
        public string PaddingStyles { get; set; }
    }

    public class PageListPreviewViewModel
    {
        public PageData Page { get; set; }
        public string Template { get; set; }
        public string PreviewOption { get; set; }
        public bool ShowIntroduction { get; set; }
        public bool ShowPublishDate { get; set; }

        public PageListPreviewViewModel(PageData page, PageListBlock block)
        {
            Page = page;
            Template = block.Template;
            PreviewOption = block.PreviewOption;
            ShowIntroduction = block.IncludeTeaserText;
            ShowPublishDate = block.IncludePublishDate;
        }
    }
}
