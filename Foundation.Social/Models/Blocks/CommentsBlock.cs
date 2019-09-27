using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Social.Models.Blocks
{
    /// <summary>
    /// The CommentBlock class defines the configuration used for rendering comments views.
    /// </summary>
    [ContentType(DisplayName = "Comments Block",
                 GUID = "b8a110ff-a8e2-4c17-9706-ce777694ebd4",
                 Description = "Configures the frontend view properties of a comment block", GroupName = "Social")]
    [ImageUrl("~/assets/icons/cms/blocks/cms-icon-block-25.png")]
    public class CommentsBlock : BlockData
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
        /// Configures the number of rows the comment input box should have.
        /// </summary>
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual int CommentBoxRows { get; set; }

        /// <summary>
        /// Configures the max length of a comment.
        /// </summary>
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual int CommentMaxLength { get; set; }

        /// <summary>
        /// Configures the max number of comments that should be displayed in the frontend view.
        /// </summary>
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual int CommentsDisplayMax { get; set; }

        /// <summary>
        /// Configures whether an activity should be sent to the Episerver Social Activity Streams system.
        /// </summary>
        [Display(
            GroupName = SystemTabNames.Content,
            Order = 1)]
        public virtual bool SendActivity { get; set; }

        /// <summary>
        /// Sets the default configuration values.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);
            ShowHeading = false;
            Heading = "Comments";
            CommentBoxRows = 5;
            CommentMaxLength = 500;
            CommentsDisplayMax = 10;
            SendActivity = true;
        }
    }
}