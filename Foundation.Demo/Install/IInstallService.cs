using EPiServer.Core;
using EPiServer.Web;
using System.IO;

namespace Foundation.Demo.Install
{
    public interface IInstallService
    {
        FoundationConfiguration FoundationConfiguration { get; }
        InstallProgressMessenger ProgressMessenger { get; set; }
        bool ShouldInstall();
        void RunInstallSteps();
        bool ImportCatalog(Stream stream);
        bool ImportEpiserverContent(Stream stream, ContentReference destinationRoot, SiteDefinition siteDefinition = null);
        Stream ExportCatalog(string name);
        Stream ExportEpiserverContent(ContentReference root, ContentExport contentExport);
    }
}
