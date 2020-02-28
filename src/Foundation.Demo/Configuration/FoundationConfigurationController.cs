using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.Scheduler;
using EPiServer.Web;
using Foundation.Cms.Extensions;
using Foundation.Demo.Install;
using ICSharpCode.SharpZipLib.Zip;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Catalog.ImportExport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Xml;
using System.Xml.Linq;

namespace Foundation.Demo.Configuration
{
    public class FoundationConfigurationController : BaseConfigurationController
    {
        private readonly ContentExportProcessor _contentExportProcessor;
        private readonly CatalogImportExport _catalogImportExport = new CatalogImportExport
        {
            IsModelsAvailable = true
        };

        public FoundationConfigurationController(IInstallService installService,
            IStorageService storageService,
            IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            ISiteDefinitionRepository siteDefinitionRepository,
            IVisitorGroupRepository visitorGroupRepository,
            ContentExportProcessor contentExportProcessor,
            IScheduledJobExecutor scheduledJobExecutor,
            IScheduledJobRepository scheduledJobRepository) :
            base(installService, storageService, contentRepository, referenceConverter, siteDefinitionRepository, scheduledJobExecutor, scheduledJobRepository, visitorGroupRepository)
        {
            _contentExportProcessor = contentExportProcessor;
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View(GetConfigurationViewModel());
        }

        [HttpGet]
        public JsonResult GetTree(string parentId = null)
        {
            var root = ContentReference.GlobalBlockFolder;
            if (!parentId.IsNullOrEmpty())
            {
                root = ContentReference.Parse(parentId);
            }

            var nodes = ContentRepository.GetChildren<ContentFolder>(root)
                .Select(_ => new SelectListItem
                {
                    Text = _.Name,
                    Value = _.ContentLink.ToReferenceWithoutVersion().ToString(),
                    Disabled = ContentRepository.GetChildren<ContentFolder>(_.ContentLink)
                        .Any()
                })
                .ToList();

            return Json(nodes, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportCatalog(ConfigurationViewModel model)
        {
            if (model.LocalSelectedCatalogName.IsNullOrEmpty())
            {
                model = GetConfigurationViewModel();
                ModelState.AddModelError(nameof(model.LocalSelectedCatalogName), "Catalog is required.");
                return View("Index", model);
            }

            if (model.MediaFolder.IsNullOrEmpty())
            {
                model = GetConfigurationViewModel();
                ModelState.AddModelError(nameof(model.MediaFolder), "Media folder is required.");
                return View("Index", model);
            }

            var catalog = ContentRepository.GetChildren<CatalogContent>(ReferenceConverter.GetRootLink())
                .FirstOrDefault(_ => _.Name.Equals(model.LocalSelectedCatalogName));

            if (catalog == null)
            {
                model = GetConfigurationViewModel();
                ModelState.AddModelError(nameof(model.LocalSelectedCatalogName), "Catalog is required.");
                return View("Index", model);
            }

            var mediaFolder = ContentRepository.Get<ContentFolder>(ContentReference.Parse(model.MediaFolder));
            if (mediaFolder == null)
            {
                model = GetConfigurationViewModel();
                ModelState.AddModelError(nameof(model.MediaFolder), "Media folder is required.");
                return View("Index", model);
            }

            if (model.CatalogExportLocation.Equals("Remote", StringComparison.InvariantCultureIgnoreCase) && model.LocalCatalogName.IsNullOrEmpty())
            {
                model = GetConfigurationViewModel();
                ModelState.AddModelError(nameof(model.LocalCatalogName), "Blob Storage Catalog name is required.");
                return View("Index", model);
            }

            using (var stream = new MemoryStream())
            {

                _catalogImportExport.Export(model.LocalSelectedCatalogName, stream, "");
                stream.Position = 0;

                var document = XDocument.Load(stream);
                document.Element("Catalogs").Element("MetaDataScheme").ReplaceWith(GetMetaDataScheme());
                document.Element("Catalogs").Element("Dictionaries").ReplaceWith(GetDictionaries());


                var newStream = new MemoryStream();
                var writer = XmlWriter.Create(newStream);
                document.Save(writer);
                writer.Close();
                newStream.Position = 0;
                var media = InstallService.ExportEpiserverContent(mediaFolder.ContentLink, ContentExport.ExportPages);

                var zip = CreateArchiveStream(new Dictionary<string, Stream>()
                {
                    { "Catalog.xml", newStream },
                    { "ProductAssets.episerverdata", media }
                });

                var zipName = model.LocalCatalogName.IsNullOrEmpty() ? model.LocalSelectedCatalogName : model.LocalCatalogName;
                if (!zipName.EndsWith(".zip", StringComparison.InvariantCultureIgnoreCase))
                {
                    zipName += ".zip";
                }

                if (model.CatalogExportLocation.Equals("Remote", StringComparison.InvariantCultureIgnoreCase))
                {
                    StorageService.Add($"Catalogs/{zipName}", zip, zip.Length);
                    return RedirectToAction("Index");
                }

                return File(zip, "application/octet-stream", zipName);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportSite(ConfigurationViewModel model)
        {
            if (model.LocalSelectedSiteName.IsNullOrEmpty())
            {
                model = GetConfigurationViewModel();
                ModelState.AddModelError(nameof(model.LocalSelectedSiteName), "Site is required.");
                return View("Index", model);
            }

            var site = SiteDefinitionRepository.List()
                .FirstOrDefault(_ => _.Name.Equals(model.LocalSelectedSiteName));

            if (site == null)
            {
                model = GetConfigurationViewModel();
                ModelState.AddModelError(nameof(model.LocalSelectedSiteName), "Site is required.");
                return View("Index", model);
            }

            if (model.SiteExportLocation.Equals("Remote", StringComparison.InvariantCultureIgnoreCase) && model.LocalSiteName.IsNullOrEmpty())
            {
                model = GetConfigurationViewModel();
                ModelState.AddModelError(nameof(model.LocalSiteName), "Blob Storage Site name is required.");
                return View("Index", model);
            }

            var fileName = model.LocalSiteName;
            if (fileName.IsNullOrEmpty())
            {
                fileName = $"{site.Name}.episerverdata";
            }

            if (!fileName.EndsWith(".episerverdata", StringComparison.InvariantCultureIgnoreCase))
            {
                fileName += ".episerverdata";
            }

            if (model.SiteExportLocation.Equals("Remote", StringComparison.InvariantCultureIgnoreCase))
            {
                using (var exportStream = InstallService.ExportEpiserverContent(site.StartPage, ContentExport.ExportPages | ContentExport.ExportVisitorGroups))
                {
                    StorageService.Add($"Sites/{fileName}", exportStream, exportStream.Length);
                    return RedirectToAction("Index");
                }
            }

            var stream = InstallService.ExportEpiserverContent(site.StartPage, ContentExport.ExportPages | ContentExport.ExportVisitorGroups);
            return File(stream, "application/octet-stream", fileName);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddSite(ConfigurationViewModel model)
        {
            if (model.SiteName.IsNullOrEmpty())
            {
                model = GetConfigurationViewModel();
                ModelState.AddModelError(nameof(model.SelectedSiteName), "Site name is required.");
                return View("Index", model);
            }

            if (model.SiteDomainName.IsNullOrEmpty())
            {
                model = GetConfigurationViewModel();
                ModelState.AddModelError(nameof(model.SiteDomainName), "Site domain name is required.");
                return View("Index", model);
            }

            Stream siteStream = null;
            if (model.SiteImportLocation.Equals("Remote"))
            {
                if (model.SelectedSiteName.IsNullOrEmpty())
                {
                    model = GetConfigurationViewModel();
                    ModelState.AddModelError(nameof(model.SelectedSiteName), "Site is required.");
                    return View("Index", model);
                }

                var file = StorageService.GetCloudBlockBlob(new Uri(model.SelectedSiteName));
                if (!file.Exists())
                {
                    throw new Exception("Site blob does not exist");
                }
                siteStream = file.OpenRead();
            }
            else
            {
                siteStream = model.SiteImportFile?.FirstOrDefault()?.InputStream;
                if (siteStream == null)
                {
                    ModelState.AddModelError(nameof(model.SiteImportFile), "File is required for disk import.");
                    return View("Index");
                }
            }

            var domainName = model.SiteDomainName;
            if (model.SiteDomainName.StartsWith("http://", StringComparison.InvariantCultureIgnoreCase))
            {
                domainName = domainName.Replace("http://", "");
            }
            else if (model.SiteDomainName.StartsWith("https://", StringComparison.InvariantCultureIgnoreCase))
            {
                domainName = domainName.Replace("https://", "");
            }

            var siteDefinition = SiteDefinitionRepository.Get(model.SiteName);
            if (siteDefinition == null)
            {
                siteDefinition = new SiteDefinition
                {
                    Name = model.SiteName,
                    SiteUrl = new Uri($"http://{domainName}"),
                    Hosts = new List<HostDefinition>() { new HostDefinition { Name = domainName, Type = HostDefinitionType.Undefined } }
                };
            }

            CreateSite(siteStream, siteDefinition, ContentReference.IsNullOrEmpty(siteDefinition.StartPage) ? ContentReference.RootPage : siteDefinition.StartPage);
            RunIndexJob();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCatalog(ConfigurationViewModel model)
        {
            if (model.CatalogImportLocation.Equals("Remote", StringComparison.InvariantCultureIgnoreCase))
            {
                if (model.SelectedCatalogName.IsNullOrEmpty())
                {
                    model = GetConfigurationViewModel();
                    ModelState.AddModelError(nameof(model.SelectedCatalogName), "Catalog is required.");
                    return View("Index", model);
                }

                var file = StorageService.GetCloudBlockBlob(new Uri(model.SelectedCatalogName));
                if (!file.Exists())
                {
                    throw new Exception("Catalog blob does not exist");
                }

                CreateCatalog(file);
            }
            else
            {
                CreateCatalog(model.CatalogImportFile.FirstOrDefault());
            }

            RunIndexJob();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ExportVisitorGroups(ConfigurationViewModel model)
        {
            if (model.SelectedVisitorGroupItems?.Count() == 0)
            {
                model = GetConfigurationViewModel();
                ModelState.AddModelError(nameof(model.SelectedVisitorGroupItems), "SelectedVisitorGroups is required.");
                return View("Index", model);
            }

            var visitorGroupData = VisitorGroupRepository.List().Where(x => model.SelectedVisitorGroupItems.Contains(x.Id.ToString())).ToList();
            var stream = _contentExportProcessor.ExportEpiserverVisitorGroup(visitorGroupData);

            if (model.VisitorGroupExportLocation.Equals("Remote", StringComparison.InvariantCultureIgnoreCase))
            {
                var fileName = model.VisitorGroupName.IsNullOrEmpty() ? "visitorgroup.episerverdata" : model.VisitorGroupName;
                if (!fileName.EndsWith(".episerverdata", StringComparison.InvariantCultureIgnoreCase))
                {
                    fileName += ".episerverdata";
                }

                StorageService.Add($"VisitorGroups/{fileName}", stream, stream.Length);
                return RedirectToAction("Index");
            }

            return File(stream, "application/octet-stream", "visitorgroup.episerverdata");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ImportVisitorGroup(ConfigurationViewModel model)
        {
            Stream visitorGroupStream;
            if (model.VisitorGroupImportLocation.Equals("Remote"))
            {
                if (model.SelectedRemoteVisitorGroup.IsNullOrEmpty())
                {
                    model = GetConfigurationViewModel();
                    ModelState.AddModelError(nameof(model.SelectedRemoteVisitorGroup), "Visitor Group is required.");
                    return View("Index", model);
                }

                var file = StorageService.GetCloudBlockBlob(new Uri(model.SelectedRemoteVisitorGroup));
                if (!file.Exists())
                {
                    throw new Exception("Site blob does not exist");
                }
                visitorGroupStream = file.OpenRead();
            }
            else
            {
                visitorGroupStream = model.VisitorGroupImportFile?.FirstOrDefault()?.InputStream;
                if (visitorGroupStream == null)
                {
                    ModelState.AddModelError(nameof(model.VisitorGroupImportFile), "File is required for visitor group import.");
                    return View("Index");
                }
            }

            CreateVisitorGroup(visitorGroupStream, ContentReference.RootPage);

            return RedirectToAction("Index");
        }

        private XElement GetDictionaries()
        {
            return XElement.Parse(@"<Dictionaries>
                                <Merchants />
                                <Packages />
                                <TaxCategories />
                                <Warehouses />
                                <AssociationTypes />
                                <Markets />
                            </Dictionaries>");
        }

        private XElement GetMetaDataScheme() => XElement.Parse("<MetaDataScheme><MetaDataPlusBackup version=\"1.0\"></MetaDataPlusBackup></MetaDataScheme>");

        private Stream CreateArchiveStream(Dictionary<string, Stream> files)
        {
            var memoryStream = new MemoryStream();
            using (var zipOutputStream = new ZipOutputStream(memoryStream))
            {
                zipOutputStream.SetLevel(3); // 0 - store only to 9 - means best compression
                foreach (var file in files)
                {
                    if (file.Value.Position != 0)
                    {
                        file.Value.Position = 0;
                    }
                    var entry = new ZipEntry(file.Key)
                    {
                        DateTime = DateTime.Now,
                        Size = file.Value.Length
                    };
                    zipOutputStream.PutNextEntry(entry);
                    file.Value.CopyTo(zipOutputStream);
                }
                zipOutputStream.IsStreamOwner = false;
                zipOutputStream.Finish();
            }
            memoryStream.Position = 0;
            return memoryStream;
        }

        private ConfigurationViewModel GetConfigurationViewModel()
        {
            return new ConfigurationViewModel
            {
                Sites = GetBlobs("Sites"),
                LocalSites = GetLocalSites(),
                Catalogs = GetBlobs("Catalogs"),
                LocalCatalogs = GetLocalCatalogs(),
                VisitorGroups = GetBlobs("VisitorGroups"),
                LocalVisitorGroups = GetVisitorGroups(),
                SelectedVisitorGroupItems = new List<string>()
            };
        }
    }
}
