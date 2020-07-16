using Foundation.Social;
using Foundation.Social.ViewModels;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.FeedBlock
{
    public class FeedBlockViewModel
    {
        public FeedBlockViewModel(FeedBlock block)
        {
            Heading = block.Heading;
            ShowHeading = block.ShowHeading;
            FeedDisplayMax = block.FeedDisplayMax;
            FeedTitle = block.FeedTitle;
            Feed = new List<CommunityFeedItemViewModel>();
            CurrentBlock = block;
        }

        public string Heading { get; set; }

        public bool ShowHeading { get; set; }

        public int FeedDisplayMax { get; set; }

        public string FeedTitle { get; set; }

        public IEnumerable<CommunityFeedItemViewModel> Feed { get; set; }

        public List<MessageViewModel> Messages { get; set; }

        public FeedBlock CurrentBlock { get; set; }
    }
}