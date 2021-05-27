namespace Foundation.Social.Models.ActivityStreams
{
    public class CommunityFeedFilter
    {
        public string Subscriber { get; set; }

        public int PageSize { get; set; }

        public int PageOffset { get; set; }
    }
}