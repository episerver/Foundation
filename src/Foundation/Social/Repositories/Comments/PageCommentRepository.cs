using EPiServer.Social.Comments.Core;
using EPiServer.Social.Common;
using Foundation.Social.Models.Comments;
using Foundation.Social.Repositories.Common;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Social.Repositories.Comments
{
    /// <summary>
    ///     The PageCommentRepository class defines the operations that can be issued
    ///     against the Episerver Social CommentService.
    /// </summary>
    public class PageCommentRepository : IPageCommentRepository
    {
        private readonly ICommentService _commentService;
        private readonly IUserRepository _userRepository;

        /// <summary>
        ///     Constructor
        /// </summary>
        public PageCommentRepository(IUserRepository userRepository, ICommentService commentService)
        {
            _userRepository = userRepository;
            _commentService = commentService;
        }

        /// <summary>
        ///     Adds a comment with the Episerver Social Framework.
        /// </summary>
        /// <param name="comment">The comment to add.</param>
        /// <returns>The added comment.</returns>
        public PageComment Add(PageComment comment)
        {
            var newComment = AdaptPageComment(comment);
            Comment addedComment = null;

            try
            {
                addedComment = _commentService.Add(newComment);

                if (addedComment == null)
                {
                    throw new SocialRepositoryException(
                        "The newly posted comment could not be added. Please try again");
                }
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.",
                    ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException(
                    "The application request was deemed too large for Episerver Social.", ex);
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
        ///     Gets comments from the Episerver Social Framework.
        /// </summary>
        /// <param name="filter">The application comment filtering specification.</param>
        /// <returns>A list of comments.</returns>
        public IEnumerable<PageComment> Get(PageCommentFilter filter)
        {
            var comments = new List<Comment>();
            var parent = Reference.Create(filter.Target);

            try
            {
                comments = _commentService.Get(
                    new Criteria<CommentFilter>
                    {
                        PageInfo = new PageInfo
                        {
                            PageSize = filter.PageSize
                        },
                        Filter = new CommentFilter
                        {
                            Parent = parent
                        },
                        OrderBy = { new SortInfo(CommentSortFields.Created, false) }
                    }
                ).Results.ToList();
            }
            catch (SocialAuthenticationException ex)
            {
                throw new SocialRepositoryException("The application failed to authenticate with Episerver Social.",
                    ex);
            }
            catch (MaximumDataSizeExceededException ex)
            {
                throw new SocialRepositoryException(
                    "The application request was deemed too large for Episerver Social.", ex);
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
        ///     Adapt the application PageComment to the Episerver Social Comment
        /// </summary>
        /// <param name="comment">The application's PageComment.</param>
        /// <returns>The Episerver Social Comment.</returns>
        private Comment AdaptPageComment(PageComment comment)
        {
            return new Comment(Reference.Create(comment.Target), Reference.Create(comment.AuthorId), comment.Body,
                true);
        }

        /// <summary>
        ///     Adapt a Comment to PageComment.
        /// </summary>
        /// <param name="comment">The Episerver Social Comment.</param>
        /// <returns>The PageComment.</returns>
        private PageComment AdaptComment(Comment comment)
        {
            return new PageComment
            {
                AuthorId = comment.Author.ToString(),
                AuthorUsername = _userRepository.GetUserName(comment.Author.Id),
                Body = comment.Body,
                Target = comment.Parent.ToString(),
                Created = comment.Created
            };
        }

        /// <summary>
        ///     Adapt a list of Episerver Social Comment to application's PageComment.
        /// </summary>
        /// <param name="comments">The list of Episerver Social Comment.</param>
        /// <returns>The list of application PageComment.</returns>
        private IEnumerable<PageComment> AdaptComment(List<Comment> comments)
        {
            return comments.Select(c =>
                new PageComment
                {
                    AuthorId = c.Author.ToString(),
                    AuthorUsername = _userRepository.GetUserName(c.Author.Id),
                    Body = c.Body,
                    Target = c.Parent.ToString(),
                    Created = c.Created
                }
            );
        }
    }
}