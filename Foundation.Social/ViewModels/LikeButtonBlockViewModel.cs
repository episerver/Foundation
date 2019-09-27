using EPiServer.Core;

namespace Foundation.Social.ViewModels
{
    public class LikeButtonBlockViewModel
    {
        public ContentReference Link { get; set; }

        public long TotalCount { get; set; }

        public int? CurrentRating { get; set; }
    }
}