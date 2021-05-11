using System;
using System.Collections.Generic;

namespace Foundation.Social.Models.Groups
{
    public class CommunityMembershipRequest
    {
        public CommunityMembershipRequest() => Actions = new List<string>();

        public string State { get; set; }

        public string WorkflowId { get; set; }

        public DateTime Created { get; set; }

        public IEnumerable<string> Actions { get; set; }

        public string User { get; set; }

        public string Group { get; set; }

        public string UserName { get; set; }
    }
}