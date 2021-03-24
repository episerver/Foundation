using Foundation.Social.Adapters;

namespace Foundation.Social.Models.ActivityStreams
{
    public class PageCommentActivity : CommunityActivity
    {
        public string Body { get; set; }

        public override void Accept(ICommunityActivityAdapter adapter) => adapter.Visit(this);
    }
}