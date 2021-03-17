using EPiServer.Social.ActivityStreams.Core;
using EPiServer.Social.Common;
using Foundation.Social.Models.ActivityStreams;
using Foundation.Social.Repositories.Common;
using Foundation.Social.ViewModels;

namespace Foundation.Social.Adapters
{
    public class CommunityActivityAdapter : ICommunityActivityAdapter
    {
        private string _actor;
        private CommunityFeedItemViewModel _feedModel;
        private string _pageName;
        private readonly IPageRepository _pageRepository;
        private readonly IUserRepository _userRepository;

        public CommunityActivityAdapter(IUserRepository userRepository,
            IPageRepository pageRepository)
        {
            _userRepository = userRepository;
            _pageRepository = pageRepository;
        }

        public CommunityFeedItemViewModel Adapt(Composite<FeedItem, CommunityActivity> composite)
        {
            // Create and populate the CommunityFeedItemViewModel 
            _feedModel = new CommunityFeedItemViewModel
            {
                ActivityDate = composite.Data.ActivityDate
            };

            _actor = _userRepository.GetUserName(composite.Data.Actor.Id);
            _pageName = _pageRepository.GetPageName(composite.Data.Target.Id);

            // Interpret the activity
            composite.Extension.Accept(this);

            return _feedModel;
        }

        #region ISocialActivityAdapter methods

        public void Visit(PageCommentActivity activity)
        {
            // Interpret activity and set description.
            _feedModel.Heading = string.Format("{0} commented on \"{1}\".", _actor, _pageName);
            _feedModel.Description = activity.Body;
        }

        public void Visit(PageRatingActivity activity) =>
            // Interpret activity and set description.
            _feedModel.Heading = string.Format("{0} rated \"{1}\" with a {2}.", _actor, _pageName, activity.Value);

        public void Visit(CommunityActivity activity) => activity.Accept(this);

        #endregion
    }
}