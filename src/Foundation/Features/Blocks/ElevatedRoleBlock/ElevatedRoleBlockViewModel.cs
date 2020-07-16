using Foundation.Features.Shared;

namespace Foundation.Features.Blocks.ElevatedRoleBlock
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
