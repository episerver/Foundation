using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.RatingBlock
{
    /// <summary>
    /// The RatingBlock class defines the configuration used for rendering rating views.
    /// </summary>
    [ContentType(DisplayName = "Rating Block",
        GUID = "069e2c52-fd48-49c5-8993-7a0347ea1f78",
        Description = "Configures the frontend view properties of a rating block",
        GroupName = GroupNames.Social)]
    [ImageUrl("~/assets/icons/cms/blocks/cms-icon-block-25.png")]
    public class RatingBlock : FoundationBlockData
    {
        /// <summary>
        /// Configures the heading that should be used when displaying the block view in the frontend.
        /// </summary>
        [Display(GroupName = SystemTabNames.Content, Order = 10)]
        [CultureSpecific]
        public virtual string Heading { get; set; }

        /// <summary>
        /// Configures whether the heading should be displayed in the block's frontend view.
        /// </summary>
        [Display(Name = "Show heading", GroupName = SystemTabNames.Content, Order = 20)]
        public virtual bool ShowHeading { get; set; }

        /// <summary>
        /// Configures whether an activity should be sent to the Episerver Social
        /// Activity Streams system when a rating a submitted using the rating block.
        /// </summary>
        [Display(Name = "Notify on new comments", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual bool SendActivity { get; set; }

        /// <summary>
        /// Configures the list of possible rating values that can be submitted using this rating block.
        /// </summary>
        [Editable(false)]
        [ScaffoldColumn(false)]
        [Display(Name = "Rating settings", GroupName = SystemTabNames.Content, Order = 40)]
        public virtual IList<RatingSetting> RatingSettings { get; set; }

        /// <summary>
        /// Sets the default property values on the content data.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            Heading = "Ratings and Statistics";

            // By default do not display a heading on the rating block
            ShowHeading = false;

            // By default send a rating activity to the Episerver Social
            // Activity Streams system when a rating a submitted.
            SendActivity = true;

            // For the sake of the simplicity of this sample we allow items
            // to be rated on a scale of 1 through 5 by initializing this
            // non-editable property list.
            RatingSettings = new List<RatingSetting>
            {
                new RatingSetting { Value = 1 },
                new RatingSetting { Value = 2 },
                new RatingSetting { Value = 3 },
                new RatingSetting { Value = 4 },
                new RatingSetting { Value = 5 }
            };
        }
    }
}