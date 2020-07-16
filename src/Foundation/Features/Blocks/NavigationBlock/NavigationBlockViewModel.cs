using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.NavigationBlock
{
    public class NavigationBlockViewModel : BlockViewModel<NavigationBlock>
    {
        public NavigationBlockViewModel(NavigationBlock currentBlock) : base(currentBlock)
        {
            Items = new List<NavigationItem>();
            Heading = currentBlock.Heading;
        }

        public List<NavigationItem> Items { get; set; }
        public string Heading { get; set; }
    }
}
