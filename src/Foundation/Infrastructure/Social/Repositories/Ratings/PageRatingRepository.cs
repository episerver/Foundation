using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Social.Common;
using EPiServer.Social.Ratings.Core;
using Foundation.Social.Models.Ratings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Social.Repositories.Ratings
{
    /// <summary>
    ///     The PageRatingRepository class defines the operations that can be issued
    ///     against the Episerver Social RatingService.
    /// </summary>
    public class PageRatingRepository : IPageRatingRepository
    {
        private readonly CategoryRepository _categoryRepository;
        private readonly IContentRepository _contentRepository;
        private readonly IRatingService _ratingService;
        private readonly IRatingStatisticsService _ratingStatisticsService;

        /// <summary>
        ///     Constructor
        /// </summary>
        public PageRatingRepository(IRatingService ratingService,
            IRatingStatisticsService ratingStatisticsService,
            IContentRepository contentRepository,
            CategoryRepository categoryRepository)
        {
            _ratingService = ratingService;
            _ratingStatisticsService = ratingStatisticsService;
            _contentRepository = contentRepository;
            _categoryRepository = categoryRepository;
        }

        /// <summary>
        ///     Adds a rating with the Episerver Social Framework for the
        ///     target and user reference specified.
        /// </summary>
        /// <param name="user">the reference of rater who submitted the rating.</param>
        /// <param name="target">the reference of target the rating applies to.</param>
        /// <param name="value">the rating value that was submitted by the rater.</param>
        /// <exception cref="SocialRepositoryException">
        ///     Thrown when errors occur communicating with
        ///     the Social cloud services.
        /// </exception>
        public void AddRating(string user, string target, int value)
        {
            try
            {
                var rating = _ratingService.Add(new Rating(
                    Reference.Create(user),
                    Reference.Create(target),
                    new RatingValue(value)));

                if (rating == null)
                {
                    throw new SocialRepositoryException(
                        "The newly submitted rating could not be added. Please try again");
                }
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
        ///     Gets the value of the submitted rating, if any, from the Episerver Social Framework based on the target and user
        ///     reference specified in the filter.
        /// </summary>
        /// <param name="filter">
        ///     Criteria containing the target and user reference by
        ///     which to filter ratings
        /// </param>
        /// <returns>
        ///     The rating value matching the filter criteria, null otherwise, if rating
        ///     does not exist for the target and user reference specified in the filter.
        /// </returns>
        /// <exception cref="SocialRepositoryException">
        ///     Thrown when errors occur communicating with
        ///     the Social cloud services.
        /// </exception>
        public int? GetRating(PageRatingFilter filter)
        {
            int? result = null;

            try
            {
                var ratingPage = _ratingService.Get(new Criteria<RatingFilter>
                {
                    Filter = new RatingFilter
                    {
                        Rater = Reference.Create(filter.Rater),
                        Targets = new List<Reference> { Reference.Create(filter.Target) }
                    },
                    PageInfo = new PageInfo { PageSize = 1 }
                });

                if (ratingPage.Results.Any())
                {
                    result = ratingPage.Results.ToList().FirstOrDefault().Value.Value;
                }
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

            return result;
        }

        /// <summary>
        ///     Gets the rating statistics, if any, from the Episerver Social rating
        ///     repository for the specified target reference.
        /// </summary>
        /// <param name="target">The target reference by which to filter ratings statistics</param>
        /// <returns>The rating statistics if any exist, null otherwise.</returns>
        /// <exception cref="SocialRepositoryException">
        ///     Thrown when errors occur communicating with
        ///     the Social cloud services.
        /// </exception>
        public PageRatingStatistics GetRatingStatistics(string target)
        {
            PageRatingStatistics result = null;

            try
            {
                var ratingStatisticsPage = _ratingStatisticsService.Get(new Criteria<RatingStatisticsFilter>
                {
                    Filter = new RatingStatisticsFilter
                    {
                        Targets = new List<Reference> { Reference.Create(target) }
                    },
                    PageInfo = new PageInfo { PageSize = 1 }
                });

                if (ratingStatisticsPage.Results.Any())
                {
                    var statistics = ratingStatisticsPage.Results.ToList().FirstOrDefault();
                    if (statistics.TotalCount > 0)
                    {
                        result = new PageRatingStatistics
                        {
                            Average = (double)statistics.Sum / statistics.TotalCount,
                            TotalCount = statistics.TotalCount
                        };
                    }
                }
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

            return result;
        }

        /// <summary>
        ///     Gets the favorite categories for user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A dictionary with the category id and the times it occurred.</returns>
        public Dictionary<int, int> GetFavoriteCategoriesForUser(string userId)
        {
            var favoriteCategories = new Dictionary<int, int>();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return favoriteCategories;
            }

            var topRated = GetTopRatedPagesForUser(userId).ToList();

            foreach (var page in topRated.OfType<ICategorizable>())
            {
                foreach (var categoryId in page.Category)
                {
                    if (favoriteCategories.ContainsKey(categoryId))
                    {
                        favoriteCategories[categoryId] += 1;
                    }
                    else
                    {
                        favoriteCategories.Add(categoryId, 1);
                    }
                }
            }

            return favoriteCategories.OrderByDescending(c => c.Value).Take(5)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        /// <summary>
        ///     Gets the favorite content types for user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A dictionary with the content type id and the times it occurred.</returns>
        public Dictionary<int, int> GetFavoriteContentTypesForUser(string userId)
        {
            var favoriteContentTypes = new Dictionary<int, int>();

            if (string.IsNullOrWhiteSpace(userId))
            {
                return favoriteContentTypes;
            }

            var topRated = GetTopRatedPagesForUser(userId).ToList();

            foreach (var content in topRated)
            {
                var contentTypeId = content.ContentTypeID;

                if (favoriteContentTypes.ContainsKey(contentTypeId))
                {
                    favoriteContentTypes[contentTypeId] += 1;
                }
                else
                {
                    favoriteContentTypes.Add(contentTypeId, 1);
                }
            }

            return favoriteContentTypes.OrderByDescending(c => c.Value).Take(5)
                .ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        /// <summary>
        ///     Gets the top rated pages for user.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>A list of favorite content for the user.</returns>
        public IEnumerable<IContent> GetTopRatedPagesForUser(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                return new List<IContent>();
            }

            var rater = Reference.Create(userId);

            var ratingPage = _ratingService.Get(
                new Criteria<RatingFilter>
                {
                    Filter = new RatingFilter { Rater = rater },
                    PageInfo =
                        new PageInfo
                        {
                            PageSize = 25
                        },
                    OrderBy =
                        new List<SortInfo>
                        {
                            new SortInfo(RatingSortFields.Value, false),
                            new SortInfo(
                                RatingSortFields.Created,
                                false)
                        }
                });

            return ratingPage.Results.Select(result => _contentRepository.Get<IContent>(Guid.Parse(result.Target.Id)));
        }
    }
}