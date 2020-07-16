using Foundation.Social;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.MembershipAffiliationBlock
{
    public class MembershipAffiliationBlockViewModel
    {
        public MembershipAffiliationBlockViewModel(MembershipAffiliationBlock currentBlock)
        {
            Heading = currentBlock.Heading;
            ShowHeading = currentBlock.ShowHeading;
            Messages = new List<MessageViewModel>();
            Groups = new List<Social.Models.Groups.Community>();
            CurrentBlock = currentBlock;
        }

        public string Heading { get; set; }

        public bool ShowHeading { get; set; }

        public List<Social.Models.Groups.Community> Groups { get; set; }

        public List<MessageViewModel> Messages { get; set; }

        public MembershipAffiliationBlock CurrentBlock { get; set; }
    }
}