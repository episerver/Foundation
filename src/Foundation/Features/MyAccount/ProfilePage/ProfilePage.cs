using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Features.Blocks.FeedBlock;
using Foundation.Features.Blocks.MembershipAffiliationBlock;
using Foundation.Features.Shared;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Features.MyAccount.ProfilePage
{
    [ContentType(DisplayName = "Profile Page",
        GUID = "c03371fb-fc21-4a6e-8f79-68c400519145",
        Description = "Page to show and manage profile information",
        GroupName = SystemTabNames.Content,
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/elected.png")]
    public class ProfilePage : FoundationPageData
    {
        [Display(Name = "Activity feed",
            Description = "The feed section of the profile page. Local feed block will display feed items for the pages a user has subscriped to.",
            GroupName = SystemTabNames.Content,
            Order = 10)]
        public virtual FeedBlock ActivityFeed { get; set; }

        [Display(Name = "Membership affiliation",
            Description = "The membership affiliation section of the profile page. Local membership affiliation block will display the groups that the currently logged in user is a member of.",
            GroupName = SystemTabNames.Content,
            Order = 20)]
        public virtual MembershipAffiliationBlock MembershipAffiliation { get; set; }
    }
}