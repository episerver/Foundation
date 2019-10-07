using EPiServer.Social.Common;
using EPiServer.Social.Moderation.Core;
using Foundation.Social.ExtensionData;
using Foundation.Social.Models.Groups;
using Foundation.Social.Repositories.Common;
using System.Linq;

namespace Foundation.Social.Adapters
{
    public class CommunityMembershipRequestAdapter
    {
        private readonly IUserRepository _userRepository;
        private readonly Workflow _workflow;

        public CommunityMembershipRequestAdapter(Workflow workflow, IUserRepository userRepository)
        {
            _workflow = workflow;
            _userRepository = userRepository;
        }

        public CommunityMembershipRequest Adapt(Composite<WorkflowItem, AddMemberRequest> item)
        {
            var user = item.Extension.User;
            var userName = _userRepository.ParseUserUri(user);

            return new CommunityMembershipRequest
            {
                User = user,
                Group = item.Extension.Group,
                WorkflowId = item.Data.Workflow.ToString(),
                Created = item.Data.Created.ToLocalTime(),
                State = item.Data.State.Name,
                Actions = _workflow.ActionsFor(item.Data.State).Select(a => a.Name),
                UserName = userName
            };
        }
    }
}