using EPiServer.Social.Comments.Core;
using Foundation.Social.ViewModels;
using System.Collections.Generic;

namespace Foundation.Social.Services
{
    /// <summary>
    ///     The IReviewService interface describes a component capable of
    ///     managing reviews contributed for products.
    /// </summary>
    public interface IReviewService
    {
        /// <summary>
        ///     Adds a review for the identified product.
        /// </summary>
        /// <param name="productCode">Content code identifying the product being reviewed</param>
        /// <param name="review">Review to be added</param>
        ReviewViewModel Add(ReviewSubmissionViewModel review);

        /// <summary>
        ///     Gets the reviews that have been submitted for the identified product.
        /// </summary>
        /// <param name="productCode">Content code identifying the product</param>
        /// <returns>Reviews that have been submitted for the product</returns>
        ReviewsViewModel Get(string productCode);

        /// <summary>
        ///     Gets all the reviews that have been submitted for the product.
        /// </summary>
        /// <param name="visibility">
        ///     The Visibility enumeration describes the values available for filtering comments according to
        ///     their visibility
        /// </param>
        /// <returns>Reviews that have been submitted for the product</returns>
        IEnumerable<ReviewViewModel> Get(Visibility visibility, int page, int limit, out long total);

        IEnumerable<ReviewStatisticsViewModel> GetRatings(IEnumerable<string> productCodes);
    }
}