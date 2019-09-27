using EPiServer.DataAbstraction;
using EPiServer.DataAnnotations;
using Foundation.Social.Models.Blocks;
using System.ComponentModel.DataAnnotations;

namespace Foundation.Social.Models.Pages
{
    [ContentType(DisplayName = "Social Profile page",
        GUID = "c8d44748-62e6-4121-9bdb-f5574263f007",
        Description = "Page to show and manage profile information",
        GroupName = "Social",
        AvailableInEditMode = false)]
    [ImageUrl("~/assets/icons/cms/pages/elected.png")]
    public class ProfilePage : Cms.Pages.ProfilePage
    {
        [Display(
            Name = "Feed Block",
            Description = "The feed section of the profile page. Local feed block will display feed items for the pages a user has subscriped to.",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        public virtual FeedBlock Feed { get; set; }

        [Display(
            Name = "Membership Affiliation Block",
            Description = "The membership affiliation section of the profile page. Local membership affiliation block will display the groups that the currently logged in user is a member of.",
            GroupName = SystemTabNames.Content,
            Order = 2)]
        public virtual MembershipAffiliationBlock MembershipAffiliation { get; set; }
    }
}