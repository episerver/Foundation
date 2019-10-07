using EPiServer;
using EPiServer.Core;
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
            ISiteDefinitionRepository siteDefinitionRepository) :
            base(installService, storageService, contentRepository, referenceConverter, siteDefinitionRepository)
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            var model = new SetupViewModel
            {
                Catalogs = GetRemoteCatalogs(),
                Sites = GetRemoteSites()
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(SetupViewModel model)
        {
            Stream siteStream = null;

            if (model.SiteLocation.Equals("Remote", StringComparison.InvariantCultureIgnoreCase))
            {
                if (model.SiteName.IsNullOrEmpty() || model.SiteName.Equals("0"))
                {
                    ModelState.AddModelError(nameof(model.SiteName), "Site is required.");
                    Redirect("Index");
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
                    Redirect("Index");
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
                if (model.CatalogName.IsNullOrEmpty() || model.CatalogName.Equals("0"))
                {
                    return Redirect("/");
                }

                var file = StorageService.GetCloudBlockBlob(new Uri(model.CatalogName));
                if (!file.Exists())
                {
                    throw new Exception("Catalog blob does not exist");
                }

                CreateCatalog(file);

            }
            else
            {
                if (model.CatalogFile.FirstOrDefault() == null)
                {
                    return Redirect("/");
                }
                CreateCatalog(model.CatalogFile.FirstOrDefault());
            }

            var searchManager = new SearchManager(Mediachase.Commerce.Core.AppContext.Current.ApplicationName);
            searchManager.BuildIndex(true);


            return Redirect("/");
        }
    }
}