using EPiServer.Core;

namespace Foundation.Features.Blocks.LikeButtonBlock
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