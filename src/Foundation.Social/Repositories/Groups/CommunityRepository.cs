using EPiServer.Social.Common;
using EPiServer.Social.Groups.Core;
using Foundation.Social.ExtensionData;
using Foundation.Social.Models.Groups;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Social.Repositories.Groups
{
    public class CommunityRepository : ICommunityRepository
    {
        private readonly IGroupService _groupService;

        public CommunityRepository(IGroupService groupService) => _groupService = groupService;

        public Community Add(Community community)
        {
            Composite<Group, GroupExtensionData> addedGroup = null;

            try
            {
                var group = new Group(community.Name, community.Description);
                var extension = new GroupExtensionData(community.PageLink);
                addedGroup = _groupService.Add(group, extension);
                if (addedGroup == null)
                {
                    throw new SocialRepositoryException("The new community could not be added. Please try again");
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

            return new Community(addedGroup.Data.Id.Id, addedGroup.Data.Name, addedGroup.Data.Description,
                addedGroup.Extension.PageLink);
        }

        public Community Get(string communityName)
        {
            Community community = null;

            try
            {
                var criteria = new Criteria<GroupFilter>
                {
                    Filter = new GroupFilter { Name = communityName },
                    PageInfo = new PageInfo { PageSize = 1, PageOffset = 0 }
                };
                var group = _groupService.Get(criteria).Results.FirstOrDefault();
                if (group != null)
                {
                    community = new Community(group.Id.Id, group.Name, group.Description);
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

            return community;
        }

        public List<Community> Get(List<string> communityIds)
        {
            var socialGroups = new List<Community>();
            try
            {
                var groupIdList = communityIds.Select(x => GroupId.Create(x)).ToList();
                var groupCount = groupIdList.Count();
                var criteria = new CompositeCriteria<GroupFilter, GroupExtensionData>
                {
                    Filter = new GroupFilter { GroupIds = groupIdList },
                    PageInfo = new PageInfo { PageSize = groupCount },
                    OrderBy = new List<SortInfo> { new SortInfo(GroupSortFields.Name, true) }
                };
                var returnedGroups = _groupService.Get(criteria);
                socialGroups = returnedGroups.Results.Select(x =>
                    new Community(x.Data.Id.Id, x.Data.Name, x.Data.Description, x.Extension.PageLink)).ToList();
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
            catch (GroupDoesNotExistException ex)
            {
                throw new SocialRepositoryException("Episerver Social could not find the community requested.", ex);
            }

            return socialGroups;
        }

        public Community Update(Community community)
        {
            Composite<Group, GroupExtensionData> updatedGroup = null;

            try
            {
                var group = new Group(GroupId.Create(community.Id), community.Name, community.Description);
                var extension = new GroupExtensionData(community.PageLink);
                updatedGroup = _groupService.Update(group, extension);
                if (updatedGroup == null)
                {
                    throw new SocialRepositoryException("The new community could not be added. Please try again");
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

            return new Community(updatedGroup.Data.Id.Id, updatedGroup.Data.Name, updatedGroup.Data.Description,
                updatedGroup.Extension.PageLink);
        }
    }
}