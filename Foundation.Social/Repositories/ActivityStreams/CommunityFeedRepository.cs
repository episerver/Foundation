using EPiServer.Social.ActivityStreams.Core;
using EPiServer.Social.Common;
using Foundation.Social.Adapters;
using Foundation.Social.Models.ActivityStreams;
using Foundation.Social.ViewModels;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Social.Repositories.ActivityStreams
{
    /// <summary>
    ///     The CommunityFeedRepository class implements the operations for accessing feeds of community activities.
    /// </summary>
    public class CommunityFeedRepository : ICommunityFeedRepository
    {
        private readonly ICommunityActivityAdapter _activityAdapter;
        private readonly IFeedService _feedService;

        public CommunityFeedRepository(IFeedService feedService,
            ICommunityActivityAdapter adapter)
        {
            _feedService = feedService;
            _activityAdapter = adapter;
        }

        /// <summary>
        ///     Gets feed items from the underlying feed repository based on a filter.
        /// </summary>
        /// <param name="filter">a filter by which to retrieve feed items by</param>
        /// <returns>A list of feed items.</returns>
        public IEnumerable<CommunityFeedItemViewModel> Get(CommunityFeedFilter filter)
        {
            var feedItems = new List<Composite<FeedItem, CommunityActivity>>();

            try
            {
                feedItems = _feedService.Get(
                    new CompositeCriteria<FeedItemFilter, CommunityActivity>
                    {
                        PageInfo = new PageInfo
                        {
                            PageSize = filter.PageSize
                        },
                        IncludeSubclasses = true,
                        Filter = new FeedItemFilter
                        {
                            Subscriber = Reference.Create(filter.Subscriber)
                        },
                        OrderBy = { new SortInfo(FeedItemSortFields.ActivityDate, false) }
                    }
                ).Results.ToList();
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.",
                    ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException(
                    "The application request was deemed too large for Episerver Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with Episerver Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("Episerver Social failed to process the application request.", ex);
            }

            return AdaptFeedItems(feedItems);
        }

        private IEnumerable<CommunityFeedItemViewModel> AdaptFeedItems(
            List<Composite<FeedItem, CommunityActivity>> feedItems) => feedItems.Select(c => _activityAdapter.Adapt(c));
    }
}