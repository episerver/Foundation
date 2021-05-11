using EPiServer.Social.Groups.Core;
using Foundation.Social.ExtensionData;
using Foundation.Social.Models.Groups;

namespace Foundation.Social.Adapters
{
    public class CommunityMemberAdapter
    {
        public AddMemberRequest Adapt(CommunityMember member) => new AddMemberRequest(member.GroupId, member.User, member.Email, member.Company);

        public CommunityMember Adapt(AddMemberRequest memberRequest)
        {
            return new CommunityMember(memberRequest.User, memberRequest.Group, memberRequest.Email,
                memberRequest.Company);
        }

        public CommunityMember Adapt(Member member, MemberExtensionData extension) => new CommunityMember(member.User.Id, member.Group.Id, extension.Email, extension.Company);
    }
}