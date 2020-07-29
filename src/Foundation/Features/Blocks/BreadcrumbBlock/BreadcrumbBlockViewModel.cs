using Foundation.Features.Shared;
using System.Collections.Generic;

namespace Foundation.Features.Blocks.BreadcrumbBlock
{
    public class BreadcrumbBlockViewModel : BlockViewModel<BreadcrumbBlock>
    {
        public BreadcrumbBlockViewModel(BreadcrumbBlock currentBlock) : base(currentBlock)
        {
            Breadcrumb = new List<NavigationItem>();
        }

        public List<NavigationItem> Breadcrumb { get; set; }
    }
}
