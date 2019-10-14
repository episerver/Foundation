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
    [ContentType(DisplayName = "Page List Block", GUID = "30685434-33DE-42AF-88A7-3126B936AEAD", GroupName = CmsTabs.Content)]
    [ImageUrl("~/assets/icons/cms/blocks/CMS-icon-block-26.png")]
    public class PageListBlock : FoundationBlockData
    {
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        [CultureSpecific]
        public virtual string Heading { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        [DefaultValue(false)]
        public virtual bool IncludePublishDate { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 30)]
        [DefaultValue(true)]
        public virtual bool IncludeIntroduction { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 40)]
        [Required]
        public virtual int Count { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 50)]
        [DefaultValue(FilterSortOrder.PublishedDescending)]
        [UIHint("SortOrder")]
        [BackingType(typeof(PropertyNumber))]
        public virtual FilterSortOrder SortOrder { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 60)]
        [Required]
        public virtual PageReference Root { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 70)]
        public virtual PageType PageTypeFilter { get; set; }

        [Categories]
        [Display(
            Name = "Category filter (match all selected)",
            Description = "Categories to filter the list on",
            GroupName = SystemTabNames.Content,
            Order = 80)]
        public virtual IList<ContentReference> CategoryListFilter { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 90)]
        public virtual bool Recursive { get; set; }

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Count = 3;
            IncludeIntroduction = true;
            IncludePublishDate = false;
            SortOrder = FilterSortOrder.PublishedDescending;
        }
    }
}