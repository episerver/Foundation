using EPiServer.Core;

namespace Foundation.Features.Shared
{
    public interface IBlockViewModel<out T> where T : BlockData
    {
        T CurrentBlock { get; }
    }

    public class BlockViewModel<T> : IBlockViewModel<T> where T : BlockData
    {
        public BlockViewModel(T currentBlock)
        {
            CurrentBlock = currentBlock;
        }

        public T CurrentBlock { get; }
    }
}