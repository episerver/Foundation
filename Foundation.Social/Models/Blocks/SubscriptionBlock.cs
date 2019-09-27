using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Social.Models.Blocks
{
    [ContentType(DisplayName = "Subscription Block",
                 GUID = "e6b96293-60dd-46a9-8289-603f4a5e19fd",
                 Description = "Configures the properties of a subscription block frontend view", GroupName = "Social")]
    [ImageUrl("~/assets/icons/cms/blocks/cms-icon-block-25.png")]
    public class SubscriptionBlock : BlockData
    {
        /// <summary>
        /// Configures the heading that should be used when displaying the block view in the frontend.
        /// </summary>
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        [CultureSpecific]
        public virtual string Heading { get; set; }

        /// <summary>
        /// Configures whether the heading should be displayed in the block's frontend view.
        /// </summary>
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual bool ShowHeading { get; set; }

        /// <summary>
        /// Sets the default configuration values.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            ShowHeading = false;
            Heading = "Page Subscription";
        }
    }
}