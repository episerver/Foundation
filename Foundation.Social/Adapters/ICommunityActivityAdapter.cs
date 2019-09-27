using EPiServer.Social.ActivityStreams.Core;
using EPiServer.Social.Common;
using Foundation.Social.Models.ActivityStreams;
using Foundation.Social.ViewModels;

namespace Foundation.Social.Adapters
{
    public interface ICommunityActivityAdapter
    {
        CommunityFeedItemViewModel Adapt(Composite<FeedItem, CommunityActivity> composite);

        void Visit(CommunityActivity activity);

        void Visit(PageCommentActivity activity);

        void Visit(PageRatingActivity activity);
    }
}