using Foundation.Social;
using Foundation.Social.Models.Groups;
using Foundation.Social.Repositories.Common;
using Foundation.Social.Repositories.Groups;

namespace Foundation.Features.Blocks.MembershipAffiliationBlock
{
    /// <summary>
    /// The MembershipDisplayController handles the rendering of the list of members from the designated group configured in the admin view
    /// </summary>
    public class MembershipAffiliationBlockComponent : SocialBlockComponent<MembershipAffiliationBlock>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICommunityRepository _communityRepository;
        private readonly ICommunityMemberRepository _memberRepository;
        private const string ErrorMessage = "Error";

        /// <summary>
        /// Constructor
        /// </summary>
        public MembershipAffiliationBlockComponent(IUserRepository userRepository,
            ICommunityRepository communityRepository,
            ICommunityMemberRepository communityMemberRepository,
            IPageRouteHelper pageRouteHelper) : base(pageRouteHelper)
        {
            _userRepository = userRepository;
            _communityRepository = communityRepository;
            _memberRepository = communityMemberRepository;
        }

        /// <summary>
        /// Render the membership display block view.
        /// </summary>
        /// <param name="currentBlock">The current block instance.</param>
        protected override async Task<IViewComponentResult> InvokeComponentAsync(MembershipAffiliationBlock currentBlock)
        {
            //Populate model to pass to the membership affiliation view
            var membershipAffiliationBlockModel = new MembershipAffiliationBlockViewModel(currentBlock);

            try
            {
                //Retrieve the groups that are associated with the currently logged in user.
                var userId = _userRepository.GetUserId(User);
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    var memberFilter = new CommunityMemberFilter
                    {
                        UserId = _userRepository.CreateAuthenticatedUri(userId),
                        PageSize = currentBlock.NumberOfMembers
                    };
                    var listOfSocialMembers = _memberRepository.Get(memberFilter);
                    GetAffiliatedGroups(membershipAffiliationBlockModel, listOfSocialMembers);
                }
                //If the user is not logged in let them know they will need to log in to see the groups they are affiliated with
                else
                {
                    var message = "Login to see the list of groups you are affiliated with.";
                    membershipAffiliationBlockModel.Messages.Add(new MessageViewModel(message, ErrorMessage));
                }
            }
            catch (SocialRepositoryException ex)
            {
                membershipAffiliationBlockModel.Messages.Add(new MessageViewModel(ex.Message, ErrorMessage));
            }

            //Return block view with populated model
            return await Task.FromResult(View("~/Features/Blocks/MembershipAffiliationBlock/MembershipAffiliationBlock.cshtml", membershipAffiliationBlockModel));
        }

        /// <summary>
        /// Populate the viewmodel with the list of social groups that a user is assoicated with
        /// </summary>
        /// <param name="membershipAffiliationBlockModel">The block viewmodel</param>
        /// <param name="listOfSocialMembers">The list of social members</param>
        private void GetAffiliatedGroups(MembershipAffiliationBlockViewModel membershipAffiliationBlockModel, IEnumerable<CommunityMember> listOfSocialMembers)
        {
            if (listOfSocialMembers != null && listOfSocialMembers.Any())
            {
                var listOfSocialGroups = _communityRepository.Get(listOfSocialMembers.Select(x => x.GroupId).ToList());
                if (listOfSocialGroups != null && listOfSocialGroups.Any())
                {
                    membershipAffiliationBlockModel.Groups = listOfSocialGroups;
                }
            }
            else
            {
                var message = "You are not affiliated with any existing groups.";
                membershipAffiliationBlockModel.Messages.Add(new MessageViewModel(message, ErrorMessage));
            }
        }
    }
}