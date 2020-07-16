using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Shared;
using Foundation.Infrastructure;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Blocks.MembershipAffiliationBlock
{
    /// <summary>
    /// The MembershipAffiliationBlock class defines the configuration used for the list of groups that a member is associated with.
    /// </summary>
    [ContentType(DisplayName = "Membership Affiliation Block",
        GUID = "d7f22a41-a26c-4e85-b4a5-15929d4222fc",
        Description = "Configures the properties of a membership affiliation block view",
        GroupName = GroupNames.Social)]
    [ImageUrl("~/assets/icons/cms/blocks/cms-icon-block-25.png")]
    public class MembershipAffiliationBlock : FoundationBlockData
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
        /// Configures the maximum number of members that should be displayed in the view.
        /// </summary>
        [Display(Name = "Number of members", GroupName = SystemTabNames.Content, Order = 30)]
        public virtual int NumberOfMembers { get; set; }

        /// <summary>
        /// Sets the default property values on the content data.
        /// </summary>
        /// <param name="contentType">Type of the content.</param>
        public override void SetDefaultValues(ContentType contentType)
        {
            base.SetDefaultValues(contentType);

            Heading = "Membership Affiliation";
            ShowHeading = false;
            NumberOfMembers = 10;
        }
    }
}