using Foundation.Social.Adapters;

namespace Foundation.Social.Models.ActivityStreams
{
    public interface ICommunityActivity
    {
        void Accept(ICommunityActivityAdapter adapter);
    }
}