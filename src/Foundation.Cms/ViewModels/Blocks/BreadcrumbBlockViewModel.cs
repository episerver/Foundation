using Foundation.Cms.Blocks;
using System.Collections.Generic;

namespace Foundation.Cms.ViewModels.Blocks
{
    public class BreadcrumbBlockViewModel : BlockViewModel<BreadcrumbBlock>
    {
        public BreadcrumbBlockViewModel(BreadcrumbBlock currentBlock) : base(currentBlock) => Breadcrumb = new List<NavigationItem>();

        public List<NavigationItem> Breadcrumb { get; set; }
    }
}
