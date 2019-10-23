using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Filters;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms.EditorDescriptors;
using Geta.EpiCategories.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Blog List Block", GUID = "4149A1C1-5DE7-4BAF-899A-D7F09636AB32", GroupName = CmsGroupNames.Blog, AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/blocks/cms-icon-block-28.png")]
    public class BlogListBlock : FoundationBlockData
    {
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual PageReference Root { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual bool Recursive { get; set; }

        [UIHint("SortOrder")]
        [BackingType(typeof(PropertyNumber))]
        [DefaultValue(FilterSortOrder.PublishedDescending)]
        [Display(Name = "Sort order", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual FilterSortOrder SortOrder { get; set; }

        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Heading { get; set; }

        [Display(Name = "Include publish date", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual bool IncludePublishDate { get; set; }

        [Display(Name = "Include teaser text", GroupName = SystemTabNames.Content, Order = 50)]
        public virtual bool IncludeTeaserText { get; set; }

        [Display(Name = "Page type filter", GroupName = SystemTabNames.Content, Order = 60)]
        public virtual PageType PageTypeFilter { get; set; }

        [Categories]
        [Display(
            Name = "Category filter (match all selected)",
            Description = "Categories to filter the list on",
            GroupName = SystemTabNames.Content,
            Order = 70)]
        public virtual IList<ContentReference> CategoryListFilter { get; set; }

        [Display(Name = "Template of blogs listing", GroupName = SystemTabNames.Content, Order = 80)]
        [SelectOne(SelectionFactoryType = typeof(TemplateBlogListSelectionFactory))]
        public virtual string Template { get; set; }

        [Display(Name = "Preview option (not available in the Grid template)", GroupName = SystemTabNames.Content, Order = 90)]
        [SelectOne(SelectionFactoryType = typeof(PreviewOptionSelectionFactory))]
        public virtual string PreviewOption { get; set; }


        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            Template = TemplateSelections.Grid;
            PreviewOption = PreviewOptions.Full;
            IncludeTeaserText = true;
            IncludePublishDate = true;
            SortOrder = FilterSortOrder.PublishedDescending;
            Recursive = true;
        }
    }
}