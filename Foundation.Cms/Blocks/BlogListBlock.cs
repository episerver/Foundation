using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using EPiServer.Filters;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(GUID = "4149A1C1-5DE7-4BAF-899A-D7F09636AB32", DisplayName = "Blog List Block", GroupName = "Blog", AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/blocks/cms-icon-block-28.png")]
    public class BlogListBlock : BlockData
    {
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        [CultureSpecific]
        public virtual string Heading { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 2, Name = "Include Publish Date")]
        [DefaultValue(false)]
        public virtual bool IncludePublishDate { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 3, Name = "Include Introduction")]
        [DefaultValue(true)]
        public virtual bool IncludeIntroduction { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 4, Name = "Sort Order")]
        [DefaultValue(FilterSortOrder.PublishedDescending)]
        [UIHint("SortOrder")]
        [BackingType(typeof(PropertyNumber))]
        public virtual FilterSortOrder SortOrder { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 5)]
        public virtual PageReference Root { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 6, Name = "Page Type Filter")]
        public virtual PageType PageTypeFilter { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 7, Name = "Category Filter")]
        public virtual CategoryList CategoryFilter { get; set; }

        [Display(
            GroupName = SystemTabNames.Content,
            Order = 8)]
        public virtual bool Recursive { get; set; }

        #region IInitializableContent

        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            IncludeIntroduction = true;
            IncludePublishDate = true;
            SortOrder = FilterSortOrder.PublishedDescending;
            Recursive = true;
        }

        #endregion
    }
}