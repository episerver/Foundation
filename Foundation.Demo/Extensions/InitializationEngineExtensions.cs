using EPiServer.Framework.Initialization;
using Foundation.Demo.Install;

namespace Foundation.Demo.Extensions
{
    public static class InitializationEngineExtensions
    {
        public static void InitializeFoundationDemo(this InitializationEngine context)
        {
            var installService = context.Locate.Advanced.GetInstance<IInstallService>();
            if (!installService.ShouldInstall())
            {
                return;
            }
            installService.RunInstallSteps();
        }
    }
}
