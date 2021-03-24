using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using Foundation.Social.Adapters;
using Foundation.Social.ExtensionData;
using Foundation.Social.Models.Groups;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Social.Repositories.Groups
{
    /// <summary>
    ///     CommunityMemberRepository persists and retrieves community member data to and from the Episerver Social Framework
    /// </summary>
    public class CommunityMemberRepository : ICommunityMemberRepository
    {
        private readonly IMemberService _memberService;
        private readonly CommunityMemberAdapter _communityMemberAdapter;

        /// <summary>
        ///     Constructor
        /// </summary>
        public CommunityMemberRepository(IMemberService memberService)
        {
            _memberService = memberService;
            _communityMemberAdapter = new CommunityMemberAdapter();
        }

        /// <summary>
        ///     Adds a member to the Episerver Social Framework.
        /// </summary>
        /// <param name="communityMember">The member to add.</param>
        /// <returns>The added member.</returns>
        public CommunityMember Add(CommunityMember communityMember)
        {
            CommunityMember addedSocialMember = null;

            try
            {
                var userReference = Reference.Create(communityMember.User);
                var groupId = GroupId.Create(communityMember.GroupId);
                var member = new Member(userReference, groupId);
                var extensionData = new MemberExtensionData(communityMember.Email, communityMember.Company);
                var addedCompositeMember = _memberService.Add(member, extensionData);
                addedSocialMember =
                    _communityMemberAdapter.Adapt(addedCompositeMember.Data, addedCompositeMember.Extension);

                if (addedSocialMember == null)
                {
                    throw new SocialRepositoryException("The new member could not be added. Please try again");
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

            return addedSocialMember;
        }

        /// <summary>
        ///     Retrieves a page of community members from the Episerver Social Framework.
        /// </summary>
        /// <param name="communityMemberFilter">The filter by which to retrieve members by</param>
        /// <returns>The list of members that are part of the specified group.</returns>
        public IEnumerable<CommunityMember> Get(CommunityMemberFilter communityMemberFilter)
        {
            IEnumerable<CommunityMember> returnedMembers = null;

            try
            {
                var compositeFilter = BuildCriteria(communityMemberFilter);

                var compositeMember = _memberService.Get(compositeFilter).Results;
                returnedMembers = compositeMember.Select(x => _communityMemberAdapter.Adapt(x.Data, x.Extension));
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

            return returnedMembers;
        }

        /// <summary>
        ///     Build the appropriate CompositeCriteria based the provided CommunityMemberFilter.
        ///     The member filter will either contain a group id or a logged in user id. If neitheris provided an exception is
        ///     thrown.
        /// </summary>
        /// <param name="communityMemberFilter">The provided member filter</param>
        /// <returns>A composite criteria of type MemberFilter and MemberExtensionData</returns>
        private CompositeCriteria<MemberFilter, MemberExtensionData> BuildCriteria(
            CommunityMemberFilter communityMemberFilter)
        {
            var pageInfo = new PageInfo { PageSize = communityMemberFilter.PageSize };
            var orderBy = new List<SortInfo> { new SortInfo(MemberSortFields.Id, false) };
            var compositeCriteria = new CompositeCriteria<MemberFilter, MemberExtensionData>
            {
                PageInfo = pageInfo,
                OrderBy = orderBy
            };

            if (!string.IsNullOrEmpty(communityMemberFilter.CommunityId) &&
                string.IsNullOrEmpty(communityMemberFilter.UserId))
            {
                compositeCriteria.Filter = new MemberFilter { Group = GroupId.Create(communityMemberFilter.CommunityId) };
            }
            else if (!string.IsNullOrEmpty(communityMemberFilter.UserId) &&
                     string.IsNullOrEmpty(communityMemberFilter.CommunityId))
            {
                compositeCriteria.Filter = new MemberFilter { User = Reference.Create(communityMemberFilter.UserId) };
            }
            else
            {
                throw new SocialException(
                    "This implementation of a CommunityMemberFilter should only contain either a CommunityId or a UserReference.");
            }

            return compositeCriteria;
        }
    }
}