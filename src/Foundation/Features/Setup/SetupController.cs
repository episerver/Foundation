using EPiServer;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.Scheduler;
using EPiServer.Web;
using Foundation.Cms.Extensions;
using Foundation.Demo.Configuration;
using Foundation.Demo.Install;
using Mediachase.Commerce.Catalog;
using Mediachase.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Foundation.Features.Setup
{
    public class SetupController : BaseConfigurationController
    {
        public SetupController(IInstallService installService,
            IStorageService storageService,
            IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            ISiteDefinitionRepository siteDefinitionRepository,
            IVisitorGroupRepository visitorGroupRepository,
            IScheduledJobExecutor scheduledJobExecutor,
            IScheduledJobRepository scheduledJobRepository) :
            base(installService, storageService, contentRepository, referenceConverter, siteDefinitionRepository, scheduledJobExecutor, scheduledJobRepository, visitorGroupRepository)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new SetupViewModel
            {
                Catalogs = GetRemoteCatalogs(),
                Sites = GetRemoteSites(),
                VisitorGroups = GetRemoteVisitorGroups()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SetupViewModel model)
        {
            Stream siteStream = null;
            Stream visitorGroupStream = null;

            if (model.SiteLocation.Equals("Remote", StringComparison.InvariantCultureIgnoreCase))
            {
                if (model.SiteName.IsNullOrEmpty() || model.SiteName.Equals("0"))
                {
                    ModelState.AddModelError(nameof(model.SiteName), "Site is required.");
                    return RedirectToAction("Index");
                }

                var file = StorageService.GetCloudBlockBlob(new Uri(model.SiteName));
                if (!file.Exists())
                {
                    throw new Exception("Site blob does not exist");
                }
                siteStream = file.OpenRead();
            }
            else
            {
                siteStream = model.SiteFile?.FirstOrDefault()?.InputStream;
                if (siteStream == null)
                {
                    ModelState.AddModelError(nameof(model.SiteFile), "File is required for local import.");
                    return RedirectToAction("Index");
                }
            }

            if (siteStream != null)
            {
                var siteDefinition = new SiteDefinition
                {
                    Name = InstallService.FoundationConfiguration.ApplicationName,
                    SiteUrl = new Uri($"http://{InstallService.FoundationConfiguration.ApplicationName}"),
                    Hosts = new List<HostDefinition>()
                    {
                        new HostDefinition { Name = "*", Type = HostDefinitionType.Undefined }
                    }
                };

                if (!InstallService.FoundationConfiguration.SitePublicDomain.IsNullOrEmpty() &&
                    !InstallService.FoundationConfiguration.ApplicationName.Equals(InstallService.FoundationConfiguration.SitePublicDomain))
                {
                    siteDefinition.Hosts.Add(new HostDefinition
                    {
                        Name = InstallService.FoundationConfiguration.SitePublicDomain,
                        Type = HostDefinitionType.Primary
                    });
                }
                else
                {
                    siteDefinition.Hosts.Add(new HostDefinition
                    {
                        Name = InstallService.FoundationConfiguration.ApplicationName,
                        Type = HostDefinitionType.Primary
                    });
                }
                CreateSite(siteStream, siteDefinition, ContentReference.RootPage);
            }


            if (model.CatalogLocation.Equals("Remote", StringComparison.InvariantCultureIgnoreCase))
            {
                if ((model.CatalogName.IsNullOrEmpty() || model.CatalogName.Equals("0")) == false)
                {
                    var file = StorageService.GetCloudBlockBlob(new Uri(model.CatalogName));
                    if (!file.Exists())
                    {
                        throw new Exception("Catalog blob does not exist");
                    }
                    CreateCatalog(file);
                }
            }
            else
            {
                if (model.CatalogFile.FirstOrDefault() != null)
                {
                    CreateCatalog(model.CatalogFile.FirstOrDefault());
                }
            }

            if (model.VisitorGroupLocation.Equals("Remote", StringComparison.InvariantCultureIgnoreCase))
            {
                if ((model.VisitorGroupName.IsNullOrEmpty() || model.VisitorGroupName.Equals("0")) == false)
                {
                    var file = StorageService.GetCloudBlockBlob(new Uri(model.VisitorGroupName));
                    if (!file.Exists())
                    {
                        throw new Exception("Visitor group blob does not exist");
                    }
                    visitorGroupStream = file.OpenRead();
                    CreateVisitorGroup(visitorGroupStream, ContentReference.RootPage);
                }
            }
            else
            {
                visitorGroupStream = model.VisitorGroupFile?.FirstOrDefault()?.InputStream;
                if (visitorGroupStream != null)
                {
                    CreateVisitorGroup(visitorGroupStream, ContentReference.RootPage);
                }
            }

            var searchManager = new SearchManager(Mediachase.Commerce.Core.AppContext.Current.ApplicationName);
            searchManager.BuildIndex(true);

            var config = EPiServer.Find.Configuration.GetConfiguration();
            if (!config.ServiceUrl.Equals("https://es-us-api01.episerver.com/9IKGqgMZaTD9KP4Op3ygsVB6JeJzR0N6") && !config.DefaultIndex.Equals("episerverab_index55794"))
            {
                RunIndexJob();
            }

            return Redirect("/");
        }
    }
}