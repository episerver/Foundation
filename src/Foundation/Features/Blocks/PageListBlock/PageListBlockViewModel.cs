using EPiServer.Core;
using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.PageListBlock
{
    public class PageListBlockViewModel : BlockViewModel<PageListBlock>
    {
        public PageListBlockViewModel(PageListBlock block) : base(block)
        {
            Heading = block.Heading;
            ShowIntroduction = block.IncludeTeaserText;
            ShowPublishDate = block.IncludePublishDate;
            Padding = block.Padding;
            SetPreviewOptionValue(block.PreviewOption);
        }

        public string Heading { get; set; }
        public IEnumerable<PageListPreviewViewModel> Pages { get; set; }
        public bool ShowIntroduction { get; set; }
        public bool ShowPublishDate { get; set; }
        public string Padding { get; set; }
        public int PreviewOption { get; set; }
        private void SetPreviewOptionValue(string option)
        {
            //Set the correct column width
            if (option.Equals("1/3"))
                PreviewOption = 4;
            else if (option.Equals("1/2"))
                PreviewOption = 6;
            else if (option.Equals("1"))
                PreviewOption = 12;
        }
    }

    public class PageListPreviewViewModel
    {
        public PageData Page { get; set; }
        public string Template { get; set; }
        public string PreviewOption { get; set; }
        public bool ShowIntroduction { get; set; }
        public bool ShowPublishDate { get; set; }
        public bool Flip { get; set; }
        public bool Highlight { get; set; }

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
