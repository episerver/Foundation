using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Blocks.CommentsBlock;
using Foundation.Features.Blocks.GroupAdmissionBlock;
using Foundation.Features.Blocks.MembershipDisplayBlock;
using Foundation.Features.Blocks.RatingBlock;
using Foundation.Features.Blocks.SubscriptionBlock;
using Foundation.Features.Shared;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.Community
{
    /// <summary>
    /// Used for the pages that wish to contain Social community features
    /// </summary>
    [ContentType(DisplayName = "Community Page", GUID = "56ba715e-3fb9-4050-a5e3-ab7fe1690742", Description = "A reseller's community page using Episerver Social.", GroupName = "Social")]
    [ImageUrl("~/assets/icons/cms/pages/elected.png")]
    public class CommunityPage : FoundationPageData
    {
        /// <summary>
        /// The comment section of the page. Local comment block will display comments only for this page
        /// </summary>
        [Display(Name = "Comment block",
            Description = "The comment section of the page. Local comment block will display comments only for this page",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual CommentsBlock Comments { get; set; }

        /// <summary>
        /// The comment section of the page. Local ratings block will allow a logged in user to rate this page
        /// </summary>
        [Display(Name = "Ratings block",
            Description = "The comment section of the page. Local ratings block will allow a logged in user to rate this page",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual RatingBlock Ratings { get; set; }

        /// <summary>
        /// The subscription section of the page. Local subscription block will allow a logged in user to subscribe to this page
        /// </summary>
        [Display(Name = "Subscription block",
            Description = "The subscription section of the page. Local subscription block will allow a logged in user to subscribe to this page",
            GroupName = SystemTabNames.Content,
            Order = 30)]
        public virtual SubscriptionBlock Subscriptions { get; set; }

        /// <summary>
        /// The membership display section of the page. Local membership display block will display existing membership for the group that corresponds to this page
        /// </summary>
        [Display(Name = "Membership display block",
            Description = "The membership display section of the page. Local membership display block will display existing membership for the group that corresponds to this page",
            GroupName = SystemTabNames.Content,
            Order = 40)]
        public virtual MembershipDisplayBlock Memberships { get; set; }

        /// <summary>
        /// The group admission section of the page. Local group creation block will allow a logged in user to submit a request for membrship admission for the group that corresponds to this page
        /// </summary>
        [Display(Name = "Group admission block",
            Description = "The group admission section of the page. Local group creation block will allow a logged in user to submit a request for membrship admission for the group that corresponds to this page",
            GroupName = SystemTabNames.Content,
            Order = 50)]
        public virtual GroupAdmissionBlock GroupAdmission { get; set; }
    }
}
