using EPiServer.Framework.DataAnnotations;
using EPiServer.Web.Routing;
using Foundation.Features.Community;
using Foundation.Social;
using Foundation.Social.Models.ActivityStreams;
using Foundation.Social.Models.Groups;
using Foundation.Social.Models.Ratings;
using Foundation.Social.Repositories.ActivityStreams;
using Foundation.Social.Repositories.Common;
using Foundation.Social.Repositories.Groups;
using Foundation.Social.Repositories.Ratings;
using Foundation.Social.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Blocks.RatingBlock
{
    /// <summary>
    /// The RatingBlockController handles the rendering of any existing rating statistics
    /// for the page on which the RatingBlock resides.
    /// This controller also allows a logged in user to rate a page which that user has not
    /// yet rated or view the rating that user has already submitted in the past for that page.
    /// </summary>
    [TemplateDescriptor(Default = true)]
    public class RatingBlockController : SocialBlockController<RatingBlock>
    {
        private readonly IUserRepository _userRepository;
        private readonly IPageRatingRepository _ratingRepository;
        private readonly ICommunityActivityRepository _activityRepository;
        private readonly IPageRepository _pageRepository;
        private readonly ICommunityRepository _communityRepository;
        private readonly ICommunityMemberRepository _memberRepository;
        private string _userId;
        private string _pageId;
        private const string MessageKey = "RatingBlock";
        private const string ErrorMessage = "Error";
        private const string SuccessMessage = "Success";

        /// <summary>
        /// Constructor
        /// </summary>
        public RatingBlockController(
            IUserRepository userRepository,
            IPageRatingRepository ratingRepository,
            IPageRepository pageRepository,
            ICommunityActivityRepository activityRepository,
            ICommunityRepository communityRepository,
            ICommunityMemberRepository memberRepository,
            IPageRouteHelper pageRouteHelper) : base(pageRouteHelper)
        {
            _userRepository = userRepository;
            _ratingRepository = ratingRepository;
            _pageRepository = pageRepository;
            _activityRepository = activityRepository;
            _communityRepository = communityRepository;
            _memberRepository = memberRepository;
        }

        /// <summary>
        /// Render the rating block frontend view.
        /// </summary>
        /// <param name="currentBlock">The current frontend block instance.</param>
        /// <returns>The index action result.</returns>
        public override ActionResult Index(RatingBlock currentBlock)
        {
            var target = _pageRouteHelper.Page.ContentGuid.ToString();

            var groupName = _pageRouteHelper.Page is CommunityPage
                            ? ((CommunityPage)_pageRouteHelper.Page).Memberships.GroupName
                            : "";

            var group = string.IsNullOrEmpty(groupName)
                        ? null
                        : _communityRepository.Get(groupName);

            var currentPageLink = _pageRouteHelper.PageLink;

            // Create a rating block view model to fill the frontend block view
            var blockModel = new RatingBlockViewModel(currentBlock, currentPageLink)
            {
                //get messages for view
                Messages = RetrieveMessages(MessageKey)
            };

            // If user logged in, check if logged in user has already rated the page
            if (User.Identity.IsAuthenticated)
            {
                //Validate that the group exists
                if (group != null)
                {
                    var groupId = group.Id;
                    var memberFilter = new CommunityMemberFilter
                    {
                        CommunityId = groupId,
                        PageSize = 10000
                    };
                    var socialMembers = _memberRepository.Get(memberFilter).ToList();
                    var userId = _userRepository.GetUserId(User);
                    blockModel.IsMemberOfGroup = socialMembers.FirstOrDefault(m => m.User.IndexOf(userId) > -1) != null;
                }
                GetRating(target, blockModel);
            }

            //Conditionally retrieving rating statistics based on any errors that might have been encountered
            var noMessages = blockModel.Messages.Count == 0;
            var noErrors = blockModel.Messages.Any(x => x.Type != ErrorMessage);
            if (noMessages || noErrors)
            {
                GetRatingStatistics(target, blockModel);
            }

            return PartialView("~/Features/Blocks/RatingBlock/RatingBlock.cshtml", blockModel);
        }

        /// <summary>
        /// Submit handles the submission of a new rating.  It accepts a rating form model,
        /// stores the submitted rating, and redirects back to the current page.
        /// </summary>
        /// <param name="ratingForm">The rating form that was submitted.</param>
        /// <returns>The submit action result.</returns>
        [HttpPost]
        public ActionResult Submit(RatingFormViewModel ratingForm)
        {
            ValidateSubmitRatingForm(ratingForm);

            // Add the rating and verify success
            var addRatingSuccess = AddRating(ratingForm.SubmittedRating.Value);

            if (addRatingSuccess && ratingForm.SendActivity)
            {
                // Add a rating activity
                AddActivity(ratingForm.SubmittedRating.Value);
            }
            return Redirect(UrlResolver.Current.GetUrl(ratingForm.CurrentLink));
        }

        /// <summary>
        /// Gets the rating for the logged in user
        /// </summary>
        /// <param name="target">The current page on which the RatingBlock resides</param>
        /// <param name="blockModel">a reference to the RatingBlockViewModel to
        /// populate with rating for the logged in user and errors, if any</param>
        private void GetRating(string target, RatingBlockViewModel blockModel)
        {
            blockModel.CurrentRating = null;

            try
            {
                var userId = _userRepository.GetUserId(User);
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    blockModel.CurrentRating =
                        _ratingRepository.GetRating(new PageRatingFilter
                        {
                            Rater = userId,
                            Target = target
                        });
                }
                else
                {
                    var message = "There was an error identifying the logged in user. Please make sure you are logged in and try again.";
                    blockModel.Messages.Add(new MessageViewModel(message, ErrorMessage));
                }
            }
            catch (SocialRepositoryException ex)
            {
                blockModel.Messages.Add(new MessageViewModel(ex.Message, ErrorMessage));
            }
        }

        /// <summary>
        /// Gets the rating statistics for the page on which the RatingBlock resides
        /// </summary>
        /// <param name="target">The current page on which the RatingBlock resides</param>
        /// <param name="blockModel">a reference to the RatingBlockViewModel to
        /// populate with rating statistics for the current page and errors, if any</param>
        private void GetRatingStatistics(string target, RatingBlockViewModel blockModel)
        {
            blockModel.NoStatisticsFoundMessage = string.Empty;

            try
            {
                var result = _ratingRepository.GetRatingStatistics(target);
                if (result != null)
                {
                    blockModel.Average = result.Average;
                    blockModel.TotalCount = result.TotalCount;
                }
                else
                {
                    var loggedInMessage = "This page has not been rated. Be the first!";
                    var loggedOutMessage = "This page has not been rated. Log in and be the first!";
                    var loggedInNotMemberMessage = "This page has not been rated. Join group and be the first!";
                    blockModel.NoStatisticsFoundMessage = User.Identity.IsAuthenticated
                                                            ? (blockModel.IsMemberOfGroup ? loggedInMessage : loggedInNotMemberMessage)
                                                            : loggedOutMessage;
                }
            }
            catch (SocialRepositoryException ex)
            {
                blockModel.Messages.Add(new MessageViewModel(ex.Message, ErrorMessage));
            }
        }

        /// <summary>
        /// Adds the rating submitted by the logged in user
        /// </summary>
        /// <param name="value">The value of the submitted rating</param>
        private bool AddRating(int value)
        {
            try
            {
                _ratingRepository.AddRating(_userId, _pageId, value);
                var message = "Thank you for submitting your rating!";
                AddMessage(MessageKey, new MessageViewModel(message, SuccessMessage));
                return true;
            }
            catch (SocialRepositoryException ex)
            {
                AddMessage(MessageKey, new MessageViewModel(ex.Message, ErrorMessage));
            }
            return false;
        }

        /// <summary>
        /// Adds an activity corresponding to the rating submitted action by the logged in user
        /// </summary>
        /// <param name="value">The value of the submitted rating</param>
        private void AddActivity(int value)
        {
            try
            {
                var activity = new PageRatingActivity { Value = value };
                _activityRepository.Add(_userId, _pageId, activity);
            }
            catch (SocialRepositoryException ex)
            {
                AddMessage(MessageKey, new MessageViewModel(ex.Message, ErrorMessage));
            }
        }

        /// <summary>
        /// Validates the rating that was submitted.
        /// </summary>
        /// <param name="ratingForm">The rating form that was submitted.</param>
        private void ValidateSubmitRatingForm(RatingFormViewModel ratingForm)
        {
            var message = string.Empty;
            // Validate user is logged in
            if (!User.Identity.IsAuthenticated)
            {
                message = "Session timed out, you have to be logged in to submit your rating. Please login and try again.";
            }
            else
            {
                // Validate a rating was submitted
                if (!ratingForm.SubmittedRating.HasValue)
                {
                    message = "Please select a valid rating";
                }
                else
                {
                    // Retrieve and validate the page identifier of the page that was rated
                    _pageId = _pageRepository.GetPageId(ratingForm.CurrentLink);
                    if (string.IsNullOrWhiteSpace(_pageId))
                    {
                        message = "The page id of this page could not be determined. Please try rating this page again.";
                    }
                    else
                    {
                        // Retrieve and validate the user identifier of the rater
                        _userId = _userRepository.GetUserId(User);
                        if (string.IsNullOrWhiteSpace(_userId))
                        {
                            message = "There was an error identifying the logged in user. Please make sure you are logged in and try again.";
                        }
                    }
                }
            }
            AddMessage(MessageKey, new MessageViewModel(message, ErrorMessage));
        }
    }
}
