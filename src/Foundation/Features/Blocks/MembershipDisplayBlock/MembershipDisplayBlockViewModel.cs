using Foundation.Social;
using Foundation.Social.ViewModels;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.MembershipDisplayBlock
{
    public class MembershipDisplayBlockViewModel
    {
        public MembershipDisplayBlockViewModel(MembershipDisplayBlock currentBlock)
        {
            Heading = currentBlock.Heading;
            ShowHeading = currentBlock.ShowHeading;
            GroupName = currentBlock.GroupName;
            Messages = new List<MessageViewModel>();
            Members = new List<CommunityMemberViewModel>();
            CurrentBlock = currentBlock;
        }

        public string Heading { get; set; }

        public bool ShowHeading { get; set; }

        public string GroupName { get; set; }

        public List<CommunityMemberViewModel> Members { get; set; }

        public List<MessageViewModel> Messages { get; set; }

        public MembershipDisplayBlock CurrentBlock { get; set; }
    }
}