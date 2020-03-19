using Foundation.Cms.ViewModels;
using Foundation.Commerce.Models.Blocks;

namespace Foundation.Commerce.ViewModels
{
    public class ElevatedRoleBlockViewModel : IBlockViewModel<ElevatedRoleBlock>
    {
        public ElevatedRoleBlockViewModel(ElevatedRoleBlock block)
        {
            CurrentBlock = block;
            IsAccess = false;
        }

        public ElevatedRoleBlock CurrentBlock { get; }
        public bool IsAccess { get; set; }
    }
}
