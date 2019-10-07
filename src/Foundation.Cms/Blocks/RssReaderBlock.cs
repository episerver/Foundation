using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Cms.Blocks
{
    [ContentType(DisplayName = "RSS Reader Block", GUID = "8fc5a3bb-727c-4871-8b2e-5ff337e30e82", Description = "Display content from a RSS feed", GroupName = "Syndication")]
    [ImageUrl("~/assets/icons/gfx/block-type-thumbnail-rss.png")]
    public class RssReaderBlock : FoundationBlockData
    {
        [CultureSpecific]
        [Editable(true)]
        [Display(
            Name = "RSS Feed URL",
            Description = "URL for RSS feed",
            GroupName = SystemTabNames.Content,
            Order = 1)]
        [Required]
        public virtual Url RssUrl { get; set; }

        [Editable(true)]
        [Display(
            Name = "Heading",
            Description = "Heading for the RSS feed",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        [CultureSpecific]
        public virtual String Heading { get; set; }

        [Editable(true)]
        [Display(
            Name = "Descriptive Text",
            Description = "Descriptive text for the RSS feed",
            GroupName = SystemTabNames.Content,
            Order = 3)]
        [CultureSpecific]
        public virtual XhtmlString DescriptiveText { get; set; }

        [Editable(true)]
        [Display(
            Name = "Max Count",
            Description = "Maximum number of items to display",
            GroupName = SystemTabNames.Content,
            Order = 4)]
        public virtual int MaxCount { get; set; }

        [Editable(true)]
        [Display(
            Name = "Include Publish Date",
            Description = "Include publish date for each item in list",
            GroupName = SystemTabNames.Content,
            Order = 5)]
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