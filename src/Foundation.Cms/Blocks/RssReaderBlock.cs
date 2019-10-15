using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "RSS Reader Block", 
        GUID = "8fc5a3bb-727c-4871-8b2e-5ff337e30e82", 
        Description = "Display content from a RSS feed", 
        GroupName = CmsTabNames.Content)]
    [ImageUrl("~/assets/icons/gfx/block-type-thumbnail-rss.png")]
    public class RssReaderBlock : FoundationBlockData
    {
        [Required]
        [Editable(true)]
        [CultureSpecific]
        [Display(Name = "RSS feed URL", Description = "URL for RSS feed", GroupName = SystemTabNames.Content, Order = 10)]
        public virtual Url RssUrl { get; set; }

        [Editable(true)]
        [CultureSpecific]
        [Display(Description = "Heading for the RSS feed", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual string Heading { get; set; }

        [Editable(true)]
        [CultureSpecific]
        [Display(Name = "Descriptive text", Description = "Descriptive text for the RSS feed", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual XhtmlString DescriptiveText { get; set; }

        [Editable(true)]
        [Display(Name = "Max count", Description = "Maximum number of items to display", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual int MaxCount { get; set; }

        [Editable(true)]
        [Display(Name = "Include publish date", Description = "Include publish date for each item in list", GroupName = SystemTabNames.Content, Order = 50)]
        public virtual bool IncludePublishDate { get; set; }

        /// <summary>
        /// Sets the default property values on the content data.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            MaxCount = 5;
            IncludePublishDate = false;
        }
    }
}