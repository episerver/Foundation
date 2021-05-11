using Foundation.Social.Models.Comments;
using System.Collections.Generic;

namespace Foundation.Social.Repositories.Comments
{
    /// <summary>
    /// The IBlogCommentRepository interface defines the operations that can be issued
    /// against a blog comment repository.
    /// </summary>
    public interface IBlogCommentRepository
    {
        /// <summary>
        /// Adds a comment to the underlying comment repository.
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        /// <returns>The added comment.</returns>
        BlogComment Add(BlogComment comment);

        /// <summary>
        /// Gets comments from the underlying comment repository based on a filter.
        /// </summary>
        /// <param name="filter"></param>
        /// <returns>A list of comments.</returns>
        IEnumerable<BlogComment> Get(PageCommentFilter filter, out long total);
    }
}