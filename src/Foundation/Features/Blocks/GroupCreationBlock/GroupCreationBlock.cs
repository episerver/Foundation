using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.GroupCreationBlock
{
    /// <summary>
    /// The GroupCreationBlock class defines the configuration used for rendering group creation views.
    /// </summary>
    [ContentType(DisplayName = "Group Creation Block",
        GUID = "efed721d-05bf-4d69-8e27-b907699a13c3",
        Description = "Configures the properties of a group creation block view",
        GroupName = GroupNames.Social)]
    [ImageUrl("~/assets/icons/cms/blocks/cms-icon-block-25.png")]
    public class GroupCreationBlock : BlockData
    {
        /// <summary>
        /// Configures the heading that should be used when displaying the block view.
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
        /// Sets the default property values on the content data.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            ShowHeading = false;
            Heading = "Group Creation";
        }
    }
}