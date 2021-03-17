using EPiServer.Core;
using Foundation.Social.Models.Ratings;
using System.Collections.Generic;

namespace Foundation.Social.Repositories.Ratings
{
    /// <summary>
    ///     The IPageRatingRepository interface defines the operations that can be issued
    ///     against a rating repository.
    /// </summary>
    public interface IPageRatingRepository
    {
        /// <summary>
        ///     Gets the value of the submitted rating, if any, based on the target and user reference specified in the filter.
        /// </summary>
        /// <param name="filter">
        ///     Criteria containing the target and user reference by
        ///     which to filter ratings
        /// </param>
        /// <returns>
        ///     The rating value matching the filter criteria, null otherwise, if rating
        ///     does not exist for the target and user reference specified in the filter.
        /// </returns>
        int? GetRating(PageRatingFilter filter);

        /// <summary>
        ///     Gets the rating statistics, if any, for the specified target reference.
        /// </summary>
        /// <param name="target">The target reference by which to filter ratings statistics</param>
        /// <returns>The rating statistics if any exist, null otherwise.</returns>
        PageRatingStatistics GetRatingStatistics(string target);

        /// <summary>
        ///     Adds a rating for the target and user reference specified.
        /// </summary>
        /// <param name="user">the reference of rater who submitted the rating.</param>
        /// <param name="target">the reference of target the rating applies to.</param>
        /// <param name="value">the rating value that was submitted by the rater.</param>
        void AddRating(string user, string target, int value);

        IEnumerable<IContent> GetTopRatedPagesForUser(string userId);
        Dictionary<int, int> GetFavoriteCategoriesForUser(string userId);
        Dictionary<int, int> GetFavoriteContentTypesForUser(string userId);
    }
}