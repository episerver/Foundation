using EPiServer.Core;

namespace Foundation.Features.Blog.BlogCommentBlock
{
    /// <summary>
    /// A view model for submitting a BlogComment.
    /// </summary>
    public class BlogCommentFormViewModel
    {
        /// <summary>
        /// Default parameterless constructor required for view form submitting.
        /// </summary>
        public BlogCommentFormViewModel()
        {
        }

        /// <summary>
        /// The comment name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The comment email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The comment body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets the reference link of the page containing the comment form.
        /// </summary>
        public PageReference CurrentPageLink { get; set; }
    }
}