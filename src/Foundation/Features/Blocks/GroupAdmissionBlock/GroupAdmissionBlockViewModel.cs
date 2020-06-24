using EPiServer.Core;
using Foundation.Social;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.GroupAdmissionBlock
{
    public class GroupAdmissionBlockViewModel
    {
        public GroupAdmissionBlockViewModel(GroupAdmissionBlock block, ContentReference currentLink)
        {
            Heading = block.Heading;
            ShowHeading = block.ShowHeading;
            CurrentLink = currentLink;
        }

        public GroupAdmissionBlockViewModel()
        {
        }

        public string Heading { get; set; }

        public bool ShowHeading { get; set; }

        public List<MessageViewModel> Messages { get; set; }

        public string MemberName { get; set; }

        public string MemberEmail { get; set; }

        public string MemberCompany { get; set; }

        public bool IsModerated { get; set; }

        public bool UserIsLoggedIn { get; set; }

        public string ModeratedUserAdmissionState { get; set; }

        public string GroupId { get; set; }

        public string GroupName { get; set; }

        public ContentReference CurrentLink { get; set; }
    }
}