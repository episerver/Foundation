using EPiServer.Web.Routing;
using Foundation.Social;
using Foundation.Social.Repositories.Groups;
using Foundation.Social.Repositories.Moderation;
using System.Web.Mvc;

namespace Foundation.Features.Blocks.GroupCreationBlock
{
    /// <summary>
    /// The GroupCreationBlockController handles rendering the Group Creation block view for adding new groups
    /// </summary>
    public class GroupCreationBlockController : SocialBlockController<GroupCreationBlock>
    {
        private readonly ICommunityRepository _communityRepository;
        private readonly ICommunityMembershipModerationRepository _moderationRepository;
        private const string MessageKey = "GroupCreationBlock";
        private const string ErrorMessage = "Error";
        private const string SuccessMessage = "Success";

        /// <summary>
        /// Constructor
        /// </summary>
        public GroupCreationBlockController(ICommunityRepository communityRepository,
            ICommunityMembershipModerationRepository moderationRepository,
            IPageRouteHelper pageRouteHelper) : base(pageRouteHelper)
        {
            _communityRepository = communityRepository;
            _moderationRepository = moderationRepository;
        }

        /// <summary>
        /// Render the GroupCreationBlock view.
        /// </summary>
        /// <param name="currentBlock">The current block instance.</param>
        public override ActionResult Index(GroupCreationBlock currentBlock)
        {
            var currentPageLink = _pageRouteHelper.PageLink;

            //Populate the model to pass to the block view
            var groupCreationBlockModel = new GroupCreationBlockViewModel(currentBlock, currentPageLink)
            {
                Messages = RetrieveMessages(MessageKey)
            };

            //Remove the existing values from the input fields
            ModelState.Clear();

            //Return the block view with populated model
            return PartialView("~/Features/Blocks/Views/GroupCreationBlock.cshtml", groupCreationBlockModel);
        }

        /// <summary>
        /// Submit handles the creation of new groups.  It accepts a GroupCreationBlockViewModel,
        /// stores the submitted group, and redirects back to the current page.
        /// </summary>
        /// <param name="model">The model submitted.</param>
        [HttpPost]
        public ActionResult Submit(GroupCreationBlockViewModel model)
        {
            AddGroup(model);
            return Redirect(UrlResolver.Current.GetUrl(model.CurrentLink));
        }

        /// <summary>
        /// Adss the group information to the underlying group repository
        /// </summary>
        /// <param name="model"></param>
        private void AddGroup(GroupCreationBlockViewModel model)
        {
            var validatedInputs = ValidateGroupInputs(model.Name, model.Description);
            if (validatedInputs)
            {
                try
                {
                    //Add the group and persist the group name in temp data to be used in the success message
                    var group = new Social.Models.Groups.Community(model.Name, model.Description);
                    var newGroup = _communityRepository.Add(group);
                    if (model.IsModerated)
                    {
                        _moderationRepository.AddWorkflow(newGroup);
                    }
                    var message = "Your group: " + model.Name + " was added successfully!";
                    AddMessage(MessageKey, new MessageViewModel(message, SuccessMessage));
                }
                catch (SocialRepositoryException ex)
                {
                    //Persist the exception message in temp data to be used in the error message
                    AddMessage(MessageKey, new MessageViewModel(ex.Message, ErrorMessage));
                }
            }
            else
            {   //Persist the exception message in temp data to be used in the error message
                var message = "Group name and description cannot be empty";
                AddMessage(MessageKey, new MessageViewModel(message, ErrorMessage));
            }
        }

        /// <summary>
        /// Validates the group name and group description properties
        /// </summary>
        /// <param name="groupName">The name of the new group</param>
        /// <param name="groupDescription">The description of the new group</param>
        /// <returns>Returns bool for if the group name and description are populated</returns>
        private bool ValidateGroupInputs(string groupName, string groupDescription) => !string.IsNullOrWhiteSpace(groupName) && !string.IsNullOrWhiteSpace(groupDescription);
    }
}