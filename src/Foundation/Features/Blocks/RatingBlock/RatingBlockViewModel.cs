using EPiServer.Core;
using Foundation.Social;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Blocks.RatingBlock
{
    public class RatingBlockViewModel
    {
        public RatingBlockViewModel(RatingBlock block, PageReference currentLink)
        {
            Heading = block.Heading;
            ShowHeading = block.ShowHeading;
            SendActivity = block.SendActivity;
            CurrentLink = currentLink;
            LoadRatingSettings(block);
            CurrentBlock = block;
        }

        public PageReference CurrentLink { get; set; }

        public string Heading { get; }

        public bool ShowHeading { get; set; }

        public List<int> RatingSettings { get; set; }

        public long TotalCount { get; set; }

        public double Average { get; set; }

        public int? CurrentRating { get; set; }

        public int SubmittedRating { get; set; }

        public List<MessageViewModel> Messages { get; set; }

        public string NoStatisticsFoundMessage { get; set; }

        public bool SendActivity { get; }

        public bool IsMemberOfGroup { get; set; }

        public RatingBlock CurrentBlock { get; set; }

        private void LoadRatingSettings(RatingBlock block)
        {
            RatingSettings = new List<int>();
            RatingSettings.AddRange(block.RatingSettings.Select(r => r.Value).ToList());
            RatingSettings.Sort();
        }
    }
}