using Mediachase.Commerce.Shared;

namespace Foundation.Commerce.Install
{
    public interface IInstallStep
    {
        int Order { get; }
        string Name { get; }
        bool Execute(IProgressMessenger progressMessenger);
        string Description { get; }
    }
}
