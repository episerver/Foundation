using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.Scheduler;
using EPiServer.Web;
using Foundation.Demo.Extensions;
using Foundation.Demo.Install;
using Foundation.Demo.ViewModels;
using ICSharpCode.SharpZipLib.Zip;
using Mediachase.Commerce.Catalog;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;

namespace Foundation.Demo.Configuration
{
    public class BaseConfigurationController : Controller
    {
        protected BaseConfigurationController(IInstallService installService,
            IStorageService storageService,
            IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            ISiteDefinitionRepository siteDefinitionRepository,
            IScheduledJobExecutor scheduledJobExecutor,
            IScheduledJobRepository scheduledJobRepository,
            IVisitorGroupRepository visitorGroupRepository)
        {
            InstallService = installService;
            StorageService = storageService;
            ContentRepository = contentRepository;
            ReferenceConverter = referenceConverter;
            SiteDefinitionRepository = siteDefinitionRepository;
            ScheduledJobExecutor = scheduledJobExecutor;
            ScheduledJobRepository = scheduledJobRepository;
            VisitorGroupRepository = visitorGroupRepository;
        }

        protected virtual IInstallService InstallService { get; }
        protected virtual IStorageService StorageService { get; }
        protected virtual IContentRepository ContentRepository { get; }
        protected virtual ReferenceConverter ReferenceConverter { get; }
        protected virtual ISiteDefinitionRepository SiteDefinitionRepository { get; }
        protected IScheduledJobExecutor ScheduledJobExecutor { get; }
        protected IScheduledJobRepository ScheduledJobRepository { get; }
        protected IVisitorGroupRepository VisitorGroupRepository { get; }

        protected virtual List<SelectListItem> GetRemoteCatalogs()
        {
            var catalogs = StorageService.GetBlobItems("Catalogs/", 0, 1000)
                .Select(_ => _.GetAzureBlob())
                .Where(_ => _.IsBlob)
                .Select(_ => new SelectListItem
                {
                    Text = _.Name,
                    Value = _.Url
                }).ToList();
            catalogs.Insert(0, new SelectListItem { Text = "Choose catalog", Value = "0" });
            return catalogs;
        }

        protected virtual List<SelectListItem> GetRemoteSites()
        {
            var sites = StorageService.GetBlobItems("Sites/", 0, 1000)
                .Select(_ => _.GetAzureBlob())
                .Where(_ => _.IsBlob)
                .Select(_ => new SelectListItem
                {
                    Text = _.Name,
                    Value = _.Url
                }).ToList();
            sites.Insert(0, new SelectListItem { Text = "Choose site", Value = "0" });
            return sites;
        }

        protected virtual List<SelectListItem> GetRemoteVisitorGroups()
        {
            var visitorGroups = StorageService.GetBlobItems("VisitorGroups/", 0, 1000)
                .Select(_ => _.GetAzureBlob())
                .Where(_ => _.IsBlob)
                .Select(_ => new SelectListItem
                {
                    Text = _.Name,
                    Value = _.Url
                }).ToList();
            visitorGroups.Insert(0, new SelectListItem { Text = "Choose visitor group", Value = "0" });
            return visitorGroups;
        }

        protected virtual List<CatalogContent> GetLocalCatalogs()
        {
            return ContentRepository.GetChildren<CatalogContent>(ReferenceConverter.GetRootLink())
                .ToList();
        }

        protected virtual List<SiteDefinition> GetLocalSites()
        {
            return SiteDefinitionRepository.List()
                 .ToList();
        }

        protected virtual List<VisitorGroup> GetVisitorGroups()
        {
            return VisitorGroupRepository.List().ToList();
        }

        protected virtual List<AzureBlob> GetBlobs(string path)
        {
            return StorageService.GetBlobItems($"{path}/", 0, 1000)
                 .Select(_ => _.GetAzureBlob())
                 .Where(_ => _.IsBlob)
                 .ToList();
        }

        protected virtual void RunIndexJob()
        {
            var job = ScheduledJobRepository.Get(new Guid("8EB257F9-FF22-40EC-9958-C1C5BA8C2A53"));
            if (job == null)
            {
                return;
            }

            ScheduledJobExecutor.StartAsync(job, new JobExecutionOptions { Trigger = ScheduledJobTrigger.User });
        }

        protected virtual void CreateSite(Stream stream, SiteDefinition siteDefinition, ContentReference startPage)
        {
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.EventedIndexingEnabled = false;
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.ScheduledPageQueueEnabled = false;
            InstallService.ImportEpiserverContent(stream, startPage, siteDefinition);
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.EventedIndexingEnabled = true;
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.ScheduledPageQueueEnabled = true;
        }

        protected virtual void CreateVisitorGroup(Stream stream, ContentReference startPage) => InstallService.ImportEpiserverContent(stream, startPage);

        protected virtual void CreateCatalog(CloudBlockBlob file)
        {
            var name = file.Name.Substring(file.Name.LastIndexOf('/') == 0 ? 0 : file.Name.LastIndexOf('/') + 1);
            var path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"..\appData\Imports\Catalog");
            var zipFile = Path.Combine(path, name);
            var zipDirectory = new DirectoryInfo(Path.Combine(path, name.Replace(".zip", "")));
            if (zipDirectory.Exists)
            {
                zipDirectory.Delete(true);
            }

            zipDirectory.Create();

            file.DownloadToFile(zipFile, FileMode.Create);
            System.IO.Compression.ZipFile.ExtractToDirectory(zipFile, zipDirectory.FullName);

            var assests = zipDirectory.GetFiles("ProductAssets*")
                .FirstOrDefault();

            var catalogXml = zipDirectory.GetFiles("*.xml")
                .FirstOrDefault();

            if (catalogXml == null || assests == null)
            {
                throw new Exception("Zip does not contain catalog.xml or ProductAssets.episerverdata");
            }

            var catalogFolder = ContentRepository.GetChildren<ContentFolder>(ContentReference.GlobalBlockFolder)
                .FirstOrDefault(_ => _.Name.Equals("Catalogs"));

            if (catalogFolder == null)
            {
                catalogFolder = ContentRepository.GetDefault<ContentFolder>(ContentReference.GlobalBlockFolder);
                catalogFolder.Name = "Catalogs";
                ContentRepository.Save(catalogFolder, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
            }

            using (var assetStream = assests.OpenRead())
            {
                EPiServer.Find.Cms.EventedIndexingSettings.Instance.EventedIndexingEnabled = false;
                EPiServer.Find.Cms.EventedIndexingSettings.Instance.ScheduledPageQueueEnabled = false;
                InstallService.ImportEpiserverContent(assetStream, catalogFolder.ContentLink);
                EPiServer.Find.Cms.EventedIndexingSettings.Instance.EventedIndexingEnabled = true;
                EPiServer.Find.Cms.EventedIndexingSettings.Instance.ScheduledPageQueueEnabled = true;
            }

            using (var catalogStream = catalogXml.OpenRead())
            {
                InstallService.ImportCatalog(catalogStream);
            }
        }

        protected virtual void CreateCatalog(HttpPostedFileBase file)
        {
            if (file == null || file.InputStream == null)
            {
                throw new Exception("File is required");
            }
            var name = file.FileName.Substring(file.FileName.LastIndexOf("\\") == 0 ? 0 : file.FileName.LastIndexOf("\\") + 1);
            var path = Path.Combine(HostingEnvironment.ApplicationPhysicalPath, @"..\appData\Imports\Catalog");
            var zipFile = Path.Combine(path, name);
            var zipDirectory = new DirectoryInfo(Path.Combine(path, name.Replace(".zip", "")));
            if (zipDirectory.Exists)
            {
                zipDirectory.Delete(true);
            }

            zipDirectory.Create();

            var zipInputStream = new ZipFile(file.InputStream);
            zipInputStream.IsStreamOwner = false;
            foreach (ZipEntry zipEntry in zipInputStream)
            {
                if (!zipEntry.IsFile)
                {
                    continue;
                }

                var entryFileName = zipEntry.Name;
                var zipStream = zipInputStream.GetInputStream(zipEntry);
                using (var fs = new FileStream(Path.Combine(zipDirectory.FullName, entryFileName), FileMode.Create, FileAccess.ReadWrite))
                {
                    zipStream.CopyTo(fs);
                }
            }


            var assests = zipDirectory.GetFiles("ProductAssets*")
                .FirstOrDefault();

            var catalogXml = zipDirectory.GetFiles("*.xml")
                .FirstOrDefault();

            if (catalogXml == null || assests == null)
            {
                throw new Exception("Zip does not contain catalog.xml or ProductAssets.episerverdata");
            }

            var catalogFolder = ContentRepository.GetChildren<ContentFolder>(ContentReference.GlobalBlockFolder)
                .FirstOrDefault(_ => _.Name.Equals("Catalogs"));

            if (catalogFolder == null)
            {
                catalogFolder = ContentRepository.GetDefault<ContentFolder>(ContentReference.GlobalBlockFolder);
                catalogFolder.Name = "Catalogs";
                ContentRepository.Save(catalogFolder, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
            }

            EPiServer.Find.Cms.EventedIndexingSettings.Instance.EventedIndexingEnabled = false;
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.ScheduledPageQueueEnabled = false;
            InstallService.ImportEpiserverContent(assests.OpenRead(), catalogFolder.ContentLink);
            InstallService.ImportCatalog(catalogXml.OpenRead());
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.EventedIndexingEnabled = true;
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.ScheduledPageQueueEnabled = true;
        }
    }
}
