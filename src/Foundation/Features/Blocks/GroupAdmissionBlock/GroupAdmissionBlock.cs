using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.GroupAdmissionBlock
{
    /// <summary>
    /// The GroupAdmissionBlock class defines the configuration used for rendering group admission views.
    /// </summary>
    [ContentType(DisplayName = "Group Admission Block",
        GUID = "611697e3-3638-445c-a45c-6454eaa5b7b1",
        Description = "Configures the properties of a group admission block view",
        GroupName = GroupNames.Social)]
    [ImageUrl("~/assets/icons/cms/blocks/cms-icon-block-25.png")]
    public class GroupAdmissionBlock : BlockData
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
        /// Sets the group for members to gain admission
        /// </summary>
        [CultureSpecific]
        [Display(Name = "Group name", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual string GroupName { get; set; }

        /// <summary>
        /// Sets the default property values on the content data.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Heading = "Group Admission";
            ShowHeading = false;
            GroupName = "Default Group";
        }
    }
}
