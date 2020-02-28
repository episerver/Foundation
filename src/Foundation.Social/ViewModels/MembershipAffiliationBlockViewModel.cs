using Foundation.Social.Models.Blocks;
using Foundation.Social.Models.Groups;
using System.Collections.Generic;

namespace Foundation.Social.ViewModels
{
    public class MembershipAffiliationBlockViewModel
    {
        public MembershipAffiliationBlockViewModel(MembershipAffiliationBlock currentBlock)
        {
            Heading = currentBlock.Heading;
            ShowHeading = currentBlock.ShowHeading;
            Messages = new List<MessageViewModel>();
            Groups = new List<Community>();
            CurrentBlock = currentBlock;
        }

        public string Heading { get; set; }

        public bool ShowHeading { get; set; }

        public List<Community> Groups { get; set; }

        public List<MessageViewModel> Messages { get; set; }

        public MembershipAffiliationBlock CurrentBlock { get; set; }
    }
}