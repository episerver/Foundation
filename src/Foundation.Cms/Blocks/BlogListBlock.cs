using System.Collections.Generic;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Filters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Geta.EpiCategories.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "Blog List Block", GUID = "4149A1C1-5DE7-4BAF-899A-D7F09636AB32", GroupName = CmsTabNames.Blog, AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/blocks/cms-icon-block-28.png")]
    public class BlogListBlock : BlockData
    {
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Heading { get; set; }

        [Display(Name = "Include publish date", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual bool IncludePublishDate { get; set; }

        [Display(Name = "Include introduction", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual bool IncludeIntroduction { get; set; }
        
        [UIHint("SortOrder")]
        [BackingType(typeof(PropertyNumber))]
        [DefaultValue(FilterSortOrder.PublishedDescending)]
        [Display(Name = "Sort order", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual FilterSortOrder SortOrder { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 50)]
        public virtual PageReference Root { get; set; }

        [Display(Name = "Page type filter", GroupName = SystemTabNames.Content, Order = 60)]
        public virtual PageType PageTypeFilter { get; set; }

        [Categories]
        [Display(
            Name = "Category filter (match all selected)",
            Description = "Categories to filter the list on",
            GroupName = SystemTabNames.Content,
            Order = 70)]
        public virtual IList<ContentReference> CategoryListFilter { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 80)]
        public virtual bool Recursive { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            IncludeIntroduction = true;
            IncludePublishDate = true;
            SortOrder = FilterSortOrder.PublishedDescending;
            Recursive = true;
        }
    }
}