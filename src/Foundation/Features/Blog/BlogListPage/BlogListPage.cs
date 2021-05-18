using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Filters;
using EPiServer.Shell.ObjectEditing;
using Foundation.Features.Shared;
using Foundation.Features.Shared.SelectionFactories;
using Foundation.Infrastructure;
//using Geta.EpiCategories.DataAnnotations;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blog.BlogListPage
{
    [ContentType(DisplayName = "Blog List Page",
        GUID = "EAADAFF2-3E89-4117-ADEB-F8D43565D2F4",
        Description = "Blog List Page for dates such as year and month",
        GroupName = GroupNames.Blog)]
    [AvailableContentTypes(Availability.Specific, Include = new[] { typeof(BlogListPage), typeof(BlogItemPage.BlogItemPage) })]
    [ImageUrl("/icons/cms/pages/cms-icon-page-20.png")]
    public class BlogListPage : FoundationPageData
    {
        [Display(GroupName = SystemTabNames.Content, Order = 5)]
        public virtual string Heading { get; set; }

        [Display(GroupName = TabNames.BlogList, Order = 10)]
        public virtual PageReference Root { get; set; }

        [Display(Name = "Include all levels", GroupName = TabNames.BlogList, Order = 20)]
        public virtual bool IncludeAllLevels { get; set; }

        [UIHint("SortOrder")]
        [BackingType(typeof(PropertyNumber))]
        [DefaultValue(FilterSortOrder.PublishedDescending)]
        [Display(Name = "Sort order", GroupName = TabNames.BlogList, Order = 30)]
        public virtual FilterSortOrder SortOrder { get; set; }

        [Display(Name = "Include publish date", GroupName = TabNames.BlogList, Order = 40)]
        public virtual bool IncludePublishDate { get; set; }

        [Display(Name = "Include teaser text", GroupName = TabNames.BlogList, Order = 50)]
        public virtual bool IncludeTeaserText { get; set; }

        //[Categories]
        [Display(
            Name = "Category filter (match all selected)",
            Description = "Categories to filter the list on",
            GroupName = TabNames.BlogList,
            Order = 70)]
        public virtual IList<ContentReference> CategoryListFilter { get; set; }

        [Display(Name = "Template of blogs listing", GroupName = TabNames.BlogList, Order = 80)]
        [SelectOne(SelectionFactoryType = typeof(TemplateListSelectionFactory))]
        public virtual string Template { get; set; }

        [Display(Name = "Preview option (not available in the Grid template)", GroupName = TabNames.BlogList, Order = 90)]
        [SelectOne(SelectionFactoryType = typeof(PreviewOptionSelectionFactory))]
        public virtual string PreviewOption { get; set; }

        [Display(Name = "Overlay color (hex or rgba)", Description = "Apply for Card template", GroupName = TabNames.BlogList, Order = 100)]
        [ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        public virtual string OverlayColor { get; set; }

        [Display(Name = "Overlay text color (hex or rgba)", Description = "Apply for Card template", GroupName = TabNames.BlogList, Order = 110)]
        [ClientEditor(ClientEditingClass = "foundation/editors/ColorPicker")]
        public virtual string OverlayTextColor { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            IncludeAllLevels = true;
            Template = TemplateSelections.Grid;
            PreviewOption = PreviewOptions.Full;
            IncludeTeaserText = true;
            IncludePublishDate = true;
            SortOrder = FilterSortOrder.PublishedDescending;
            OverlayColor = "rgba(34,61,107,.95)";
            OverlayTextColor = "#ffffff";
        }
    }
}