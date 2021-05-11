using EPiServer.Social.ActivityStreams.Core;
using EPiServer.Social.Common;
using Foundation.Social.Models.ActivityStreams;

namespace Foundation.Social.Repositories.ActivityStreams
{
    /// <summary>
    ///     This CommunityActivityRepository defines the operations used to add activities pretaining to community actions with
    ///     the Episerver Social Framework.
    /// </summary>
    public class CommunityActivityRepository : ICommunityActivityRepository
    {
        private readonly IActivityService _service;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="service">An instance of the Episerver Social ActivityService</param>
        public CommunityActivityRepository(IActivityService service) => _service = service;

        /// <summary>
        ///     Adds an activity to the Episerver Social Activity Streams system.
        /// </summary>
        /// <param name="actor">the actor who initiated the activity</param>
        /// <param name="target">the target of the activity</param>
        /// <param name="activity">an instance of CommunityActivity</param>
        /// <exception cref="SocialRepositoryException">
        ///     Thrown when errors occur
        ///     interacting with the Social cloud services.
        /// </exception>
        public void Add(string actor, string target, CommunityActivity activity)
        {
            try
            {
                _service.Add(new Activity(
                        Reference.Create(actor),
                        Reference.Create(target)), activity
                );
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
        }
    }
}