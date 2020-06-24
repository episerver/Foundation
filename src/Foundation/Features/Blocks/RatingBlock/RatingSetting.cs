namespace Foundation.Features.Blocks.RatingBlock
{
    /// <summary>
    /// This class is used by the rating block to encapsulate one of the
    /// possible rating values that can be submitted using that rating block
    /// </summary>
    public class RatingSetting
    {
        /// <summary>
        /// Gets or sets the rating value that can be submitted
        /// </summary>
        public int Value { get; set; }
    }
}