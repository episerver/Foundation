using Foundation.Social.Models.Groups;
using Foundation.Social.Models.Moderation;
using System.Collections.Generic;

namespace Foundation.Social.ViewModels
{
    public class CommunityModerationViewModel
    {
        public CommunityModerationViewModel()
        {
            Workflows = new List<CommunityMembershipWorkflow>();
            Items = new List<CommunityMembershipRequest>();
        }

        public IEnumerable<CommunityMembershipWorkflow> Workflows { get; set; }
        public CommunityMembershipWorkflow SelectedWorkflow { get; set; }
        public IEnumerable<CommunityMembershipRequest> Items { get; set; }
    }
}