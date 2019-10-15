using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Social.Models.Blocks
{
    [ContentType(DisplayName = "Feed Block",
        GUID = "2bb4ac6d-6f09-4d38-adb0-5dc2bcf310ac",
        Description = "Configures the properties of a feed block frontend view",
        GroupName = SocialTabNames.Social)]
    [ImageUrl("~/assets/icons/cms/blocks/cms-icon-block-25.png")]
    public class FeedBlock : BlockData
    {
        /// <summary>
        /// Configures the heading that should be used when displaying the block view in the frontend.
        /// </summary>
        [CultureSpecific]
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        public virtual string Heading { get; set; }

        /// <summary>
        /// Configures whether the heading should be displayed in the block's frontend view.
        /// </summary>
        [Display(Name = "Show heading", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual bool ShowHeading { get; set; }

        /// <summary>
        /// Configures the max number of feed items that should be displayed in the frontend view.
        /// </summary>
        [Display(Name = "Feed display max", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual int FeedDisplayMax { get; set; }

        /// <summary>
        /// Configures the title associated with any activity feed displayed for the logged in user 
        /// in the frontend feed block display.
        /// </summary>
        [Display(Name = "Feed title", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual string FeedTitle { get; set; }

        /// <summary>
        /// Sets the default configuration values.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            ShowHeading = false;
            Heading = "Activity Feed";
            FeedTitle = "Your activity feed";
            FeedDisplayMax = 20;
        }
    }
}