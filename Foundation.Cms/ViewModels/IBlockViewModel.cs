using EPiServer.Core;

namespace Foundation.Cms.ViewModels
{
    public interface IBlockViewModel<out T>
        where T : BlockData
    {
        T CurrentBlock { get; }
    }
}