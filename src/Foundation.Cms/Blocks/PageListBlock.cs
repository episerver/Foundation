using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Filters;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms.EditorDescriptors;
using Foundation.Cms.Pages;
using Geta.EpiCategories.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Page List Block", GUID = "30685434-33DE-42AF-88A7-3126B936AEAD", GroupName = SystemTabNames.Content)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-26.png")]
    public class PageListBlock : FoundationBlockData
    {
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Heading { get; set; }

        [Display(Name = "Include publish date", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual bool IncludePublishDate { get; set; }

        [Display(Name = "Include teaser text", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual bool IncludeTeaserText { get; set; }

        [Required]
        [Display(Name = "Number of results", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual int Count { get; set; }

        [UIHint("SortOrder")]
        [BackingType(typeof(PropertyNumber))]
        [Display(Name = "Sort order", GroupName = SystemTabNames.Content, Order = 50)]
        public virtual FilterSortOrder SortOrder { get; set; }

        [Required]
        [Display(GroupName = SystemTabNames.Content, Order = 60)]
        public virtual PageReference Root { get; set; }

        [Display(Name = "Filter by page type", GroupName = SystemTabNames.Content, Order = 70)]
        public virtual PageType PageTypeFilter { get; set; }

        [Categories]
        [Display(Name = "Filter by category",
            Description = "Categories to filter the list on",
            GroupName = SystemTabNames.Content,
            Order = 80)]
        public virtual IList<ContentReference> CategoryListFilter { get; set; }

        [Display(Name = "Include all levels", GroupName = SystemTabNames.Content, Order = 90)]
        public virtual bool Recursive { get; set; }

        [Display(Name = "Template of pages listing", GroupName = SystemTabNames.Content, Order = 100)]
        [SelectOne(SelectionFactoryType = typeof(TemplateBlogListSelectionFactory))]
        public virtual string Template { get; set; }

        [Display(Name = "Preview option (not available in the Grid template)", GroupName = SystemTabNames.Content, Order = 110)]
        [SelectOne(SelectionFactoryType = typeof(PreviewOptionSelectionFactory))]
        public virtual string PreviewOption { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Count = 3;
            IncludeTeaserText = true;
            IncludePublishDate = false;
            Template = TemplateSelections.Grid;
            PreviewOption = PreviewOptions.Full;
            SortOrder = FilterSortOrder.PublishedDescending;
        }
    }
}