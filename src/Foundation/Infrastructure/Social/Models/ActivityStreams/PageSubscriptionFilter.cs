namespace Foundation.Social.Models.ActivityStreams
{
    /// <summary>
    ///     The PageSubscriptionFilter class exposes a set of properties by which
    ///     social subscriptions may be filtered.
    /// </summary>
    public class PageSubscriptionFilter
    {
        /// <summary>
        ///     constructor
        /// </summary>
        public PageSubscriptionFilter()
        {
            PageSize = 10;
            PageOffset = 0;
        }

        /// <summary>
        ///     Gets or sets the subscriber.
        /// </summary>
        public string Subscriber { get; set; }

        /// <summary>
        ///     Gets or sets the target to subscribe or subscribed to.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        ///     The number of subscriptions to retrieve.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        ///     The offset to start retrieving the next page of subscriptions from.
        /// </summary>
        public int PageOffset { get; set; }
    }
}