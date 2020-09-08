using EPiServer.Core;

namespace Foundation.Features.Shared
{
    public interface IBlockViewModel<out T> where T : BlockData
    {
        T CurrentBlock { get; }
    }
}