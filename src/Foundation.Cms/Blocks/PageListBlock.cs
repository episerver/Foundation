using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Filters;
using System.ComponentModel;
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

        [Display(GroupName = SystemTabNames.Content, Order = 20)]
        public virtual bool IncludePublishDate { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 30)]
        public virtual bool IncludeIntroduction { get; set; }

        [Required]
        [Display(GroupName = SystemTabNames.Content, Order = 40)]
        public virtual int Count { get; set; }

        [UIHint("SortOrder")]
        [BackingType(typeof(PropertyNumber))]
        [Display(GroupName = SystemTabNames.Content, Order = 50)]
        public virtual FilterSortOrder SortOrder { get; set; }

        [Required]
        [Display(GroupName = SystemTabNames.Content, Order = 60)]
        public virtual PageReference Root { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 70)]
        public virtual PageType PageTypeFilter { get; set; }

        [Display(GroupName = SystemTabNames.Content, Order = 80)]
        public virtual CategoryList CategoryFilter { get; set; }

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