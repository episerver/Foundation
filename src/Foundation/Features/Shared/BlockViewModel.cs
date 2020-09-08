using EPiServer.Core;

namespace Foundation.Features.Shared
{
    public class BlockViewModel<T> : IBlockViewModel<T> where T : BlockData
    {
        public BlockViewModel(T currentBlock) => CurrentBlock = currentBlock;

        public T CurrentBlock { get; }
    }
}