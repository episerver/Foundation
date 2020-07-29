using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Routing;
using Foundation.Social;
using Foundation.Social.Models.ActivityStreams;
using Foundation.Social.Models.Comments;
using Foundation.Social.Repositories.ActivityStreams;
using Foundation.Social.Repositories.Comments;
using Foundation.Social.Repositories.Common;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Blocks.CommentsBlock
{
    [TemplateDescriptor(Default = true)]
    public class CommentsBlockController : SocialBlockController<CommentsBlock>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPageCommentRepository _commentRepository;
        private readonly IPageRepository _pageRepository;
        private readonly ICommunityActivityRepository _activityRepository;
        private const string MessageKey = "CommentBlock";
        private const string SubmitSuccessMessage = "Your comment was submitted successfully!";
        private const string BodyValidationErrorMessage = "Cannot add an empty comment.";
        private const string ErrorMessage = "Error";
        private const string SuccessMessage = "Success";

        /// <summary>
        /// Constructor
        /// </summary>
        public CommentsBlockController(IUserRepository userRepository,
            IPageCommentRepository pageCommentRepository,
            IPageRepository pageRepository,
            ICommunityActivityRepository communityActivityRepository,
            IPageRouteHelper pageRouteHelper) : base(pageRouteHelper)
        {
            _userRepository = userRepository;
            _commentRepository = pageCommentRepository;
            _pageRepository = pageRepository;
            _activityRepository = communityActivityRepository;
        }

        /// <summary>
        /// Render the comment block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current frontend block instance.</param>
        /// <returns>The action's result.</returns>
        public override ActionResult Index(CommentsBlock currentBlock)
        {
            var pageReference = _pageRouteHelper.PageLink;
            var pageId = _pageRepository.GetPageId(pageReference);

            // Create a comments block view model to fill the frontend block view
            var blockViewModel = new CommentsBlockViewModel(currentBlock, pageReference)
            {
                Messages = RetrieveMessages(MessageKey)
            };

            // Try to get recent comments
            try
            {
                var socialComments = _commentRepository.Get(
                    new PageCommentFilter
                    {
                        Target = pageId.ToString(),
                        PageSize = currentBlock.CommentsDisplayMax
                    }
                );

                blockViewModel.Comments = socialComments;
            }
            catch (SocialRepositoryException ex)
            {
                blockViewModel.Messages.Add(new MessageViewModel(ex.Message, ErrorMessage));
            }

            return PartialView("~/Features/Blocks/Views/CommentsBlock.cshtml", blockViewModel);
        }

        /// <summary>
        /// Submit handles the submitting of new comments.  It accepts a comment form model,
        /// validates the form, stores the submitted comment, sends a new activity if configuration
        /// allows, and redirects back to the current page.
        /// </summary>
        /// <param name="formViewModel">The comment form being submitted.</param>
        /// <returns>The submit action result.</returns>
        [HttpPost]
        public ActionResult Submit(CommentFormViewModel formViewModel)
        {
            var errors = ValidateCommentForm(formViewModel);

            if (errors.Count() == 0)
            {
                var addedComment = AddComment(formViewModel);
                if (addedComment != null && formViewModel.SendActivity)
                {
                    AddCommentActivity(addedComment);
                }
            }
            else
            {
                // Flag the CommentBody model state with validation error
                AddMessage(MessageKey, new MessageViewModel(errors.First(), ErrorMessage));
            }

            return Redirect(UrlResolver.Current.GetUrl(formViewModel.CurrentPageLink));
        }

        /// <summary>
        /// Adds the comment in the CommentFormViewModel to the Episerver Social repository.
        /// </summary>
        /// <param name="formViewModel">The submitted comment form view model.</param>
        /// <returns>The added PageComment</returns>
        private PageComment AddComment(CommentFormViewModel formViewModel)
        {
            var newComment = AdaptCommentFormViewModelToSocialComment(formViewModel);
            PageComment addedComment = null;

            try
            {
                addedComment = _commentRepository.Add(newComment);
                AddMessage(MessageKey, new MessageViewModel(SubmitSuccessMessage, SuccessMessage));
            }
            catch (SocialRepositoryException ex)
            {
                AddMessage(MessageKey, new MessageViewModel(ex.Message, ErrorMessage));
            }

            return addedComment;
        }

        /// <summary>
        /// Add an activity for the newly added comment.
        /// </summary>
        /// <param name="comment">The added comment.</param>
        private void AddCommentActivity(PageComment comment)
        {
            try
            {
                var commentActivity = new PageCommentActivity { Body = comment.Body };

                _activityRepository.Add(comment.AuthorId, comment.Target, commentActivity);
            }
            catch (SocialRepositoryException ex)
            {
                AddMessage(MessageKey, new MessageViewModel(ex.Message, ErrorMessage));
            }
        }

        /// <summary>
        /// Adapts a CommentFormViewModel to a PageComment.
        /// </summary>
        /// <param name="formViewModel">The comment form view model.</param>
        /// <returns>PageComment</returns>
        private PageComment AdaptCommentFormViewModelToSocialComment(CommentFormViewModel formViewModel)
        {
            return new PageComment
            {
                Target = _pageRepository.GetPageId(formViewModel.CurrentPageLink),
                Body = formViewModel.Body,
                AuthorId = _userRepository.GetUserId(User)
            };
        }

        /// <summary>
        /// Validates the comment form.
        /// </summary>
        /// <param name="formViewModel">The comment form view model.</param>
        /// <returns>Returns a list of validation errors.</returns>
        private List<string> ValidateCommentForm(CommentFormViewModel formViewModel)
        {
            var errors = new List<string>();

            // Make sure the comment body has some text
            if (string.IsNullOrWhiteSpace(formViewModel.Body))
            {
                errors.Add(BodyValidationErrorMessage);
            }

            return errors;
        }
    }
}
