using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using EPiServer.Framework.DataAnnotations;
using EPiServer.ServiceLocation;
using EPiServer.Social.Groups.Core;
using Foundation.Social;
using Foundation.Social.Models.Blocks;
using Foundation.Social.Models.Groups;
using Foundation.Social.Repositories.Common;
using Foundation.Social.Repositories.Groups;
using Foundation.Social.ViewModels;

namespace Foundation.Features.Blocks
{
    /// <summary>
    /// The MembershipDisplayController handles the rendering of the list of members from the designated group configured in the admin view
    /// </summary>
    [TemplateDescriptor(Default = true)]
    public class MembershipDisplayBlockController : SocialBlockController<MembershipDisplayBlock>
    {
        private readonly IUserRepository userRepository;
        private readonly ICommunityRepository communityRepository;
        private readonly ICommunityMemberRepository memberRepository;
        private const string ErrorMessage = "Error";

        /// <summary>
        /// Constructor
        /// </summary>
        public MembershipDisplayBlockController()
        {
            communityRepository = ServiceLocator.Current.GetInstance<ICommunityRepository>();
            memberRepository = ServiceLocator.Current.GetInstance<ICommunityMemberRepository>();
            userRepository = ServiceLocator.Current.GetInstance<IUserRepository>();
        }

        /// <summary>
        /// Render the membership display block view.
        /// </summary>
        /// <param name="currentBlock">The current block instance.</param>
        public override ActionResult Index(MembershipDisplayBlock currentBlock)
        {
            //Populate model to pass to the membership display view
            var membershipDisplayBlockModel = new MembershipDisplayBlockViewModel(currentBlock);

            //Retrieve the group id assigned to the block and populate the memberlist
            try
            {
                var group = communityRepository.Get(currentBlock.GroupName);

                //Validate that the group exists
                if (group != null)
                {
                    var groupId = group.Id;
                    var memberFilter = new CommunityMemberFilter
                    {
                        CommunityId = groupId,
                        PageSize = currentBlock.DisplayPageSize
                    };
                    var socialMembers = memberRepository.Get(memberFilter).ToList();
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

        public List<CommunityMemberViewModel> Adapt(List<CommunityMember> socialMembers)
        {
            return socialMembers.Select(x => new CommunityMemberViewModel(x.Company, this.userRepository.ParseUserUri(x.User))).ToList();
        }
    }
}
