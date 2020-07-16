using EPiServer.Core;
using Foundation.Social;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.GroupCreationBlock
{
    public class GroupCreationBlockViewModel
    {
        public GroupCreationBlockViewModel()
        {
        }

        public GroupCreationBlockViewModel(GroupCreationBlock block, ContentReference currentLink)
        {
            Heading = block.Heading;
            ShowHeading = block.ShowHeading;
            CurrentLink = currentLink;
        }

        public string Heading { get; set; }

        public bool ShowHeading { get; set; }

        public List<MessageViewModel> Messages { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public bool IsModerated { get; set; }

        public ContentReference CurrentLink { get; set; }
    }
}