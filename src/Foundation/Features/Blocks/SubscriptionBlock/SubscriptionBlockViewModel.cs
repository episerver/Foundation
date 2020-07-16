using EPiServer.Core;
using Foundation.Social;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.SubscriptionBlock
{
    public class SubscriptionBlockViewModel
    {
        public SubscriptionBlockViewModel(SubscriptionBlock block, PageReference currentLink)
        {
            Heading = block.Heading;
            ShowHeading = block.ShowHeading;
            ShowSubscriptionForm = false;
            UserSubscribedToPage = false;
            CurrentLink = currentLink;
            CurrentBlock = block;
        }

        public bool ShowSubscriptionForm { get; set; }

        public string Heading { get; set; }

        public bool ShowHeading { get; set; }

        public bool UserSubscribedToPage { get; set; }

        public PageReference CurrentLink { get; set; }

        public List<MessageViewModel> Messages { get; set; }

        public SubscriptionBlock CurrentBlock { get; set; }
    }
}