namespace Foundation.Commerce.Install
{
    public interface IInstallService
    {
        FoundationConfiguration FoundationConfiguration { get; }
        InstallProgressMessenger ProgressMessenger { get; set; }
        bool ShouldInstall();
        void RunInstallSteps();
    }
}
