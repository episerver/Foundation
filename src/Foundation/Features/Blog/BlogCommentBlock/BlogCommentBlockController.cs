using EPiServer.Core;
using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Mvc;
using EPiServer.Web.Routing;
using Foundation.Cms;
using Foundation.Social.Models.Comments;
using Foundation.Social.Repositories.Comments;
using Foundation.Social.Repositories.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Blog.BlogCommentBlock
{
    /// <summary>
    /// The CommentsBlockController handles the rendering of the comment block frontend view as well
    /// as the posting of new comments.
    /// </summary>
    [TemplateDescriptor(Default = true)]
    public class BlogCommentBlockController : BlockController<BlogCommentBlock>
    {
        private readonly IBlogCommentRepository _commentRepository;
        private readonly IPageRepository _pageRepository;
        protected readonly IPageRouteHelper _pageRouteHelper;
        private const string MessageKey = "BlogCommentBlock";
        private const string SubmitSuccessMessage = "Your comment was submitted successfully!";
        private const string BodyValidationErrorMessage = "Cannot add an empty comment.";
        private const string NameValidationErrorMessage = "Cannot add an empty name.";
        private const string EmailValidationErrorMessage = "Cannot add an empty email.";
        private const string ErrorMessage = "Error";
        private const string SuccessMessage = "Success";
        private const int RecordPerPage = 5;

        /// <summary>
        /// Constructor
        /// </summary>
        public BlogCommentBlockController(IBlogCommentRepository commentRepository, IPageRepository pageRepository, IPageRouteHelper pageRouteHelper)
        {
            _commentRepository = commentRepository;
            _pageRepository = pageRepository;
            _pageRouteHelper = pageRouteHelper;
        }

        /// <summary>
        /// Render the comment block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current frontend block instance.</param>
        /// <returns>The action's result.</returns>
        public override ActionResult Index(BlogCommentBlock currentBlock)
        {
            var pagingInfo = new PagingInfo(_pageRouteHelper.PageLink.ID, currentBlock.CommentsPerPage == 0 ? RecordPerPage : currentBlock.CommentsPerPage, 1);
            return GetComment(pagingInfo, currentBlock);
        }

        /// <summary>
        /// Render the comment block frontend view.
        /// </summary>
        /// <param name="pageId">ID of current page link that contain blogCommentBlock</param>
        /// <param name="pageIndex">Current page index of comments</param>
        /// <param name="recordPerPage">Records of comments per page</param>
        /// <returns>The action's result.</returns>
        public ActionResult GetComment(PagingInfo pagingInfo, BlogCommentBlock currentBlock)
        {
            var pageId = pagingInfo.PageId;
            var pageIndex = pagingInfo.PageNumber;
            var pageSize = pagingInfo.PageSize;

            var pageReference = new PageReference(pageId);
            var pageContentGuid = _pageRepository.GetPageId(pageReference);

            // Create a comments block view model to fill the frontend block view
            var blockViewModel = new BlogCommentsBlockViewModel(pageReference, currentBlock);

            // Try to get recent comments
            try
            {
                var blogComments = _commentRepository.Get(
                    new PageCommentFilter
                    {
                        Target = pageContentGuid.ToString(),
                        PageSize = pageSize,
                        PageOffset = pageIndex - 1
                    },
                    out var totalComments
                );

                blockViewModel.Comments = blogComments;
                blockViewModel.PagingInfo = pagingInfo;
                blockViewModel.PagingInfo.TotalRecord = (int)totalComments;
            }
            catch (Exception ex)
            {
                blockViewModel.Messages.Add(ex.Message);
            }

            return PartialView("~/Features/Blog/BlogCommentBlock/Index.cshtml", blockViewModel);
        }

        /// <summary>
        /// Submit handles the submitting of new comments.  It accepts a comment form model,
        /// validates the form, stores the submitted comment then redirects back to the current page.
        /// </summary>
        /// <param name="formViewModel">The comment form being submitted.</param>
        /// <returns>The submit action result.</returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Submit(BlogCommentFormViewModel formViewModel)
        {
            var errors = ValidateCommentForm(formViewModel);

            if (errors.Count() == 0)
            {
                var addedComment = AddComment(formViewModel);
            }
            else
            {
                // Flag the CommentBody model state with validation error
                AddMessage(MessageKey, errors.First());
            }

            return Redirect(UrlResolver.Current.GetUrl(formViewModel.CurrentPageLink));
        }

        /// <summary>
        /// Adds the comment in the CommentFormViewModel to the Episerver Social repository.
        /// </summary>
        /// <param name="formViewModel">The submitted comment form view model.</param>
        /// <returns>The added PageComment</returns>
        private BlogComment AddComment(BlogCommentFormViewModel formViewModel)
        {
            var newComment = AdaptCommentFormViewModelToSocialComment(formViewModel);
            BlogComment addedComment = null;

            try
            {
                addedComment = _commentRepository.Add(newComment);
                AddMessage(MessageKey, SubmitSuccessMessage);
            }
            catch (Exception ex)
            {
                AddMessage(MessageKey, ex.Message);
            }

            return addedComment;
        }

        /// <summary>
        /// Adapts a CommentFormViewModel to a PageComment.
        /// </summary>
        /// <param name="formViewModel">The comment form view model.</param>
        /// <returns>PageComment</returns>
        private BlogComment AdaptCommentFormViewModelToSocialComment(BlogCommentFormViewModel formViewModel)
        {
            return new BlogComment
            {
                Target = _pageRepository.GetPageId(formViewModel.CurrentPageLink),
                Name = formViewModel.Name,
                Email = formViewModel.Email,
                Body = formViewModel.Body
            };
        }

        /// <summary>
        /// Validates the comment form.
        /// </summary>
        /// <param name="formViewModel">The comment form view model.</param>
        /// <returns>Returns a list of validation errors.</returns>
        private List<string> ValidateCommentForm(BlogCommentFormViewModel formViewModel)
        {
            var errors = new List<string>();

            // Make sure the comment name has some text
            if (string.IsNullOrWhiteSpace(formViewModel.Name))
            {
                errors.Add(NameValidationErrorMessage);
            }

            // Make sure the comment email has some text
            if (string.IsNullOrWhiteSpace(formViewModel.Email))
            {
                errors.Add(EmailValidationErrorMessage);
            }

            // Make sure the comment body has some text
            if (string.IsNullOrWhiteSpace(formViewModel.Body))
            {
                errors.Add(BodyValidationErrorMessage);
            }

            return errors;
        }

        /// <summary>
        /// Used to retrieve the TempData stored for a specific controller
        /// </summary>
        /// <param name="key">Sring value of the TempData key</param>
        /// <returns>The list of MessageViewModels that was requested</returns>
        public List<string> RetrieveMessages(string key)
        {
            var listOfMessages = (List<string>)TempData[key];

            return (listOfMessages != null) && (listOfMessages.Any()) ? listOfMessages : new List<string>();
        }

        /// <summary>
        /// Stores a desired key / value in the TempData dictionary 
        /// </summary>
        /// <param name="key">The key used to reference the stored value upon retrieval</param>
        /// <param name="value">The value that is being stored in TempData</param>
        public void AddMessage(string key, string value)
        {
            var listOfMessages = RetrieveMessages(key);
            listOfMessages.Add(value);
            TempData[key] = listOfMessages;
        }
    }
}
