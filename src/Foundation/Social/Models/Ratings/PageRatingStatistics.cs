namespace Foundation.Social.Models.Ratings
{
    /// <summary>
    ///     The PageRatingStatistics class describes a rating statistics model used by the
    ///     SocialAlloy site.
    /// </summary>
    public class PageRatingStatistics
    {
        /// <summary>
        ///     Gets the average value of ratings for an item.
        /// </summary>
        public double Average { get; set; }

        /// <summary>
        ///     Gets the total number of ratings for an item.
        /// </summary>
        public long TotalCount { get; set; }
    }
}