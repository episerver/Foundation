using System;

namespace Foundation.Social.Models.Comments
{
    public class BlogComment
    {
        /// <summary>
        /// The comment author username.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The comment author email.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// The comment body.
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// The reference to the target the comment applies to.
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// The date/time the comment was created at.
        /// </summary>
        public DateTime Created { get; set; }

        public BlogComment() => Created = DateTime.Now;
    }

    public class BlogCommentExtension
    {
        public BlogCommentExtension(string email) => Email = email;

        /// <summary>
        /// The comment author email.
        /// </summary>
        public string Email { get; set; }
    }
}
