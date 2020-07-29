using EPiServer.Framework.DataAnnotations;
using EPiServer.Social.Groups.Core;
using EPiServer.Web.Routing;
using Foundation.Social;
using Foundation.Social.Models.Groups;
using Foundation.Social.Repositories.Common;
using Foundation.Social.Repositories.Groups;
using Foundation.Social.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Blocks.MembershipDisplayBlock
{
    /// <summary>
    /// The MembershipDisplayController handles the rendering of the list of members from the designated group configured in the admin view
    /// </summary>
    [TemplateDescriptor(Default = true)]
    public class MembershipDisplayBlockController : SocialBlockController<MembershipDisplayBlock>
    {
        private readonly IUserRepository _userRepository;
        private readonly ICommunityRepository _communityRepository;
        private readonly ICommunityMemberRepository _memberRepository;
        private const string ErrorMessage = "Error";

        /// <summary>
        /// Constructor
        /// </summary>
        public MembershipDisplayBlockController(ICommunityRepository communityRepository,
            ICommunityMemberRepository memberRepository,
            IUserRepository userRepository,
            IPageRouteHelper pageRouteHelper) : base(pageRouteHelper)
        {
            _communityRepository = communityRepository;
            _memberRepository = memberRepository;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Render the membership display block view.
        /// </summary>
        /// <param name="currentBlock">The current block instance.</param>
        public override ActionResult Index(MembershipDisplayBlock currentBlock)
        {
            //Populate model to pass to the membership display view
            var membershipDisplayBlockModel = new MembershipDisplayBlockViewModel(currentBlock);

            //Retrieve the group id assigned to the block and populate the member list
            try
            {
                var group = _communityRepository.Get(currentBlock.GroupName);

                //Validate that the group exists
                if (group != null)
                {
                    var groupId = group.Id;
                    var memberFilter = new CommunityMemberFilter
                    {
                        CommunityId = groupId,
                        PageSize = currentBlock.NumberOfMembers
                    };
                    var socialMembers = _memberRepository.Get(memberFilter).ToList();
                    membershipDisplayBlockModel.Members = Adapt(socialMembers);
                }
                else
                {
                    var message = "The group configured for this block cannot be found. Please update the block to use an existing group.";
                    membershipDisplayBlockModel.Messages.Add(new MessageViewModel(message, ErrorMessage));
                }
            }
            catch (SocialRepositoryException ex)
            {
                membershipDisplayBlockModel.Messages.Add(new MessageViewModel(ex.Message, ErrorMessage));
            }
            catch (GroupDoesNotExistException ex)
            {
                membershipDisplayBlockModel.Messages.Add(new MessageViewModel(ex.Message, ErrorMessage));
            }

            //Return block view with populated model
            return PartialView("~/Features/Blocks/Views/MembershipDisplayBlock.cshtml", membershipDisplayBlockModel);
        }

        public List<CommunityMemberViewModel> Adapt(List<CommunityMember> socialMembers) => socialMembers.Select(x => new CommunityMemberViewModel(x.Company, _userRepository.ParseUserUri(x.User))).ToList();
    }
}
