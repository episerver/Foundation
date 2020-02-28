using EPiServer.Core;
using Foundation.Social.Models.Blocks;

namespace Foundation.Social.ViewModels
{
    public class LikeButtonBlockViewModel
    {
        public LikeButtonBlockViewModel() : this(null)
        {

        }
        public LikeButtonBlockViewModel(LikeButtonBlock block)
        {
            CurrentBlock = block;
        }

        public ContentReference Link { get; set; }

        public long TotalCount { get; set; }

        public int? CurrentRating { get; set; }

        public LikeButtonBlock CurrentBlock { get; set; }
    }
}