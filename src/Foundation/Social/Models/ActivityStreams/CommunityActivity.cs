using Foundation.Social.Adapters;

namespace Foundation.Social.Models.ActivityStreams
{
    public abstract class CommunityActivity : ICommunityActivity
    {
        public abstract void Accept(ICommunityActivityAdapter adapter);
    }
}