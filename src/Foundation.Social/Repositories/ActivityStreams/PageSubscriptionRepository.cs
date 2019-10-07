using EPiServer.Social.ActivityStreams.Core;
using EPiServer.Social.Common;
using Foundation.Social.Models.ActivityStreams;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Social.Repositories.ActivityStreams
{
    /// <summary>
    ///     The PageSubscriptionRepository class defines the operations that can be issued
    ///     against the Episerver Social cloud subscription repository.
    /// </summary>
    public class PageSubscriptionRepository : IPageSubscriptionRepository
    {
        private readonly ISubscriptionService _subscriptionService;

        /// <summary>
        ///     Constructor
        /// </summary>
        public PageSubscriptionRepository(ISubscriptionService subscriptionService) => _subscriptionService = subscriptionService;

        /// <summary>
        ///     Adds a subscription to the Episerver Social Framework.
        /// </summary>
        /// <param name="subscription">The subscription to add.</param>
        /// <exception cref="SocialRepositoryException">
        ///     Thrown if there are any issues sending the request to the
        ///     Episerver Social Framework.
        /// </exception>
        public void Add(PageSubscription subscription)
        {
            var newSubscription = AdaptSubscription(subscription);

            try
            {
                _subscriptionService.Add(newSubscription);
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

        /// <summary>
        ///     Gets whether subscriptions exist in the Episerver Social subscription repository that match a filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>Whether subscriptions exist.</returns>
        /// <exception cref="SocialRepositoryException">
        ///     Thrown if there are any issues sending the request to the
        ///     Episerver Social subscription repository.
        /// </exception>
        public bool Exist(PageSubscriptionFilter filter)
        {
            var subscriptionFilter = AdaptSubscriptionFilter(filter);
            try
            {
                return _subscriptionService.Get(
                    new Criteria<SubscriptionFilter>
                    {
                        PageInfo = new PageInfo
                        {
                            PageSize = 0
                        },
                        Filter = subscriptionFilter
                    }
                ).HasMore;
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

        /// <summary>
        ///     Removes a subscription from the Episerver Social subscription repository.
        /// </summary>
        /// <param name="subscription">The subscription to remove.</param>
        /// <exception cref="SocialRepositoryException">
        ///     Thrown if there are any issues sending the request to the
        ///     Episerver Social cloud subscription repository.
        /// </exception>
        public void Remove(PageSubscription subscription)
        {
            var removeSubscription = AdaptSubscription(subscription);

            try
            {
                _subscriptionService.Remove(
                    new Criteria<SubscriptionFilter>
                    {
                        Filter = new SubscriptionFilter
                        {
                            Subscriber = removeSubscription.Subscriber,
                            Target = removeSubscription.Target,
                            Type = removeSubscription.Type
                        }
                    }
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

        /// <summary>
        ///     Adapt the application PageSubscription to the Episerver Social Subscription
        /// </summary>
        /// <param name="subscription">The application's PageSubscription.</param>
        /// <returns>The Episerver Social Subscription.</returns>
        private Subscription AdaptSubscription(PageSubscription subscription)
        {
            return new Subscription(Reference.Create(subscription.Subscriber),
                Reference.Create(subscription.Target));
        }

        /// <summary>
        ///     Adapt a list of Episerver Social Subscription to application's PageSubscription.
        /// </summary>
        /// <param name="subscriptions">The list of Episerver Social Subscription.</param>
        /// <returns>The list of application PageSubscription.</returns>
        private IEnumerable<PageSubscription> AdaptSocialSubscription(List<Subscription> subscriptions)
        {
            return subscriptions.Select(c =>
                new PageSubscription
                {
                    Id = c.Id.ToString(),
                    Subscriber = c.Subscriber.ToString(),
                    Target = c.Target.ToString()
                }
            );
        }

        /// <summary>
        ///     Adapt a PageSubscriptionFilter to a SubscriptionFilter
        /// </summary>
        /// <param name="filter">The PageSubscriptionFilter </param>
        /// <returns>The SubscriptionFilter</returns>
        private SubscriptionFilter AdaptSubscriptionFilter(PageSubscriptionFilter filter)
        {
            return new SubscriptionFilter
            {
                Subscriber = !string.IsNullOrWhiteSpace(filter.Subscriber)
                    ? Reference.Create(filter.Subscriber)
                    : Reference.Empty,
                Target = !string.IsNullOrWhiteSpace(filter.Target) ? Reference.Create(filter.Target) : Reference.Empty
            };
        }
    }
}