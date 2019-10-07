using Foundation.Social.Adapters;

namespace Foundation.Social.Models.ActivityStreams
{
    public class PageRatingActivity : CommunityActivity
    {
        public int Value { get; set; }

        public override void Accept(ICommunityActivityAdapter adapter) => adapter.Visit(this);
    }
}