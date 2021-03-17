using EPiServer.Social.Comments.Core;
using EPiServer.Social.Common;
using Foundation.Social.Models.Comments;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Social.Repositories.Comments
{
    /// <summary>
    /// Defines the operations on blog comment
    /// </summary>
    public class BlogCommentRepository : IBlogCommentRepository
    {
        private readonly ICommentService commentService;

        public BlogCommentRepository(ICommentService commentService) => this.commentService = commentService;

        /// <summary>
        /// Adds a comment with the Episerver Social Framework. 
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        /// <returns>The added comment.</returns>
        public BlogComment Add(BlogComment comment)
        {
            var newComment = AdaptBlogComment(comment);
            Composite<Comment, BlogCommentExtension> addedComment = null;
            var commentEtx = new BlogCommentExtension(comment.Email);

            try
            {
                addedComment = commentService.Add(newComment, commentEtx);

                if (addedComment == null)
                {
                    throw new SocialRepositoryException("The newly posted comment could not be added. Please try again");
                }
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for Episerver Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with Episerver Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("Episerver Social failed to process the application request.", ex);
            }

            return AdaptComment(addedComment);
        }

        /// <summary>
        /// Gets comments from the Episerver Social Framework.
        /// </summary>
        /// <param name="filter">The application comment filtering specification.</param>
        /// <returns>A list of comments.</returns>
        public IEnumerable<BlogComment> Get(PageCommentFilter filter, out long total)
        {
            var comments = new List<Comment>();
            var parent = EPiServer.Social.Common.Reference.Create(filter.Target);

            try
            {
                var pageComment = commentService.Get(
                    new Criteria<CommentFilter>
                    {
                        PageInfo = new PageInfo
                        {
                            PageSize = filter.PageSize,
                            CalculateTotalCount = true,
                            PageOffset = filter.PageOffset
                        },
                        Filter = new CommentFilter
                        {
                            Parent = parent
                        },
                        OrderBy = { new SortInfo(CommentSortFields.Created, false) }
                    }
                );

                total = pageComment.TotalCount;
                comments = pageComment.Results.ToList();
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.", ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException("The application request was deemed too large for Episerver Social.", ex);
            }
            catch (SocialCommunicationException ex)
            {
                throw new SocialRepositoryException("The application failed to communicate with Episerver Social.", ex);
            }
            catch (SocialException ex)
            {
                throw new SocialRepositoryException("Episerver Social failed to process the application request.", ex);
            }

            return AdaptComment(comments);
        }

        /// <summary>
        /// Adapts the application BlogComment to the Episerver Social Comment 
        /// </summary>
        /// <param name="comment">The application's BlogComment.</param>
        /// <returns>The Episerver Social Comment.</returns>
        private Comment AdaptBlogComment(BlogComment comment) => new Comment(EPiServer.Social.Common.Reference.Create(comment.Target), EPiServer.Social.Common.Reference.Create(comment.Name), comment.Body, true);

        /// <summary>
        /// Adapts a Comment to BlogComment.
        /// </summary>
        /// <param name="comment">The Episerver Social Comment.</param>
        /// <returns>The BlogComment.</returns>
        private BlogComment AdaptComment(Composite<Comment, BlogCommentExtension> comment)
        {
            return new BlogComment
            {
                Name = comment.Data.Author.ToString(),
                Email = comment.Extension.Email.ToString(),
                Body = comment.Data.Body,
                Target = comment.Data.Parent.ToString(),
                Created = comment.Data.Created
            };
        }

        /// <summary>
        /// Adapts a list of Episerver Social Comment to application's BlogComment.
        /// </summary>
        /// <param name="comments">The list of Episerver Social Comment.</param>
        /// <returns>The list of application blog comment.</returns>
        private IEnumerable<BlogComment> AdaptComment(List<Comment> comments)
        {
            return comments.Select(c =>
                new BlogComment
                {
                    Name = c.Author.ToString(),
                    Body = c.Body,
                    Target = c.Parent.ToString(),
                    Created = c.Created
                }
            );
        }
    }
}