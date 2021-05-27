using EPiServer.Core;

namespace Foundation.Social.Repositories.Common
{
    /// <summary>
    ///     This interface represents common page related operations used by the Episerver Social sample.
    /// </summary>
    public interface IPageRepository
    {
        /// <summary>
        ///     Gets the page Id given its page reference.
        /// </summary>
        /// <param name="pageLink">The page reference.</param>
        /// <returns>The page Id.</returns>
        string GetPageId(PageReference pageLink);

        /// <summary>
        ///     Gets the name of the page that has the specified identifier
        /// </summary>
        /// <param name="pageId">the page Id</param>
        /// <returns>the name of the page</returns>
        string GetPageName(string pageId);
    }
}