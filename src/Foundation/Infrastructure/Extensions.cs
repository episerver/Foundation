using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Commerce.Catalog.Linking;
using EPiServer.Commerce.Order;
using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Enterprise;
using EPiServer.Find;
using EPiServer.Find.Commerce;
using EPiServer.Logging;
using EPiServer.Scheduler;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using EPiServer.Web.Routing;
using Foundation.Cms.Extensions;
using Foundation.Cms.Settings;
using Foundation.Commerce.Customer;
using Foundation.Commerce.Customer.Services;
using Foundation.Commerce.Extensions;
using Foundation.Commerce.Install;
using Foundation.Features.CatalogContent.Product;
using Foundation.Features.CatalogContent.Variation;
using Foundation.Features.Category;
using Foundation.Features.Folder;
using Foundation.Features.Home;
using Foundation.Features.Locations.LocationItemPage;
using Foundation.Features.MyAccount.AddressBook;
using Foundation.Features.MyOrganization;
using Foundation.Features.Search;
using Foundation.Find;
using ICSharpCode.SharpZipLib.Zip;
using Mediachase.Commerce.Catalog.ImportExport;
using Mediachase.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Hosting;

namespace Foundation.Infrastructure
{
    public static class Extensions
    {
        private static readonly Lazy<IContentRepository> _contentRepository =
            new Lazy<IContentRepository>(() => ServiceLocator.Current.GetInstance<IContentRepository>());

        private static readonly Lazy<IUrlResolver> _urlResolver =
           new Lazy<IUrlResolver>(() => ServiceLocator.Current.GetInstance<IUrlResolver>());

        private static readonly Lazy<IPermanentLinkMapper> _permanentLinkMapper =
           new Lazy<IPermanentLinkMapper>(() => ServiceLocator.Current.GetInstance<IPermanentLinkMapper>());

        private static readonly Lazy<IInstallService> _installService =
           new Lazy<IInstallService>(() => ServiceLocator.Current.GetInstance<IInstallService>());

        private static readonly Lazy<ISiteDefinitionRepository> _siteDefinitionRepository =
          new Lazy<ISiteDefinitionRepository>(() => ServiceLocator.Current.GetInstance<ISiteDefinitionRepository>());

        private static readonly Lazy<IRelationRepository> RelationRepository =
          new Lazy<IRelationRepository>(() => ServiceLocator.Current.GetInstance<IRelationRepository>());

        public static FilterBuilder<T> FilterOutline<T>(this FilterBuilder<T> filterBuilder,
           IEnumerable<string> value)
        {
            var outlineFilterBuilder = new FilterBuilder<ContentData>(filterBuilder.Client);
            outlineFilterBuilder = outlineFilterBuilder.And(x => !x.MatchTypeHierarchy(typeof(EntryContentBase)));
            outlineFilterBuilder = value.Aggregate(outlineFilterBuilder,
                (current, filter) => current.Or(x => ((EntryContentBase)x).Outline().PrefixCaseInsensitive(filter)));
            return filterBuilder.And(x => outlineFilterBuilder);
        }

        public static ITypeSearch<T> FilterOutline<T>(this ITypeSearch<T> search, IEnumerable<string> value)
        {
            var filterBuilder = new FilterBuilder<T>(search.Client)
                .FilterOutline(value);

            return search.Filter(x => filterBuilder);
        }

        public static List<string> AvailableSizes(this GenericProduct genericProduct)
        {
            return genericProduct.ContentLink.GetAllVariants<GenericVariant>()
                .Select(x => x.Size)
                .Distinct()
                .ToList();
        }

        public static List<string> AvailableColors(this GenericProduct genericProduct)
        {
            return genericProduct.ContentLink.GetAllVariants<GenericVariant>()
                .Select(x => x.Color)
                .Distinct()
                .ToList();
        }

        public static IEnumerable<VariationModel> VariationModels(this ProductContent productContent)
        {
            return _contentRepository.Value
                .GetItems(productContent.GetVariants(RelationRepository.Value), productContent.Language)
                .OfType<VariationContent>()
                .Select(x => new VariationModel
                {
                    Code = x.Code,
                    LanguageId = productContent.Language.Name,
                    Name = x.DisplayName,
                    DefaultAssetUrl = (x as IAssetContainer).DefaultImageUrl()
                });
        }

        public static ContentReference GetRelativeStartPage(this IContent content)
        {
            if (content is HomePage)
            {
                return content.ContentLink;
            }

            var ancestors = _contentRepository.Value.GetAncestors(content.ContentLink);
            var startPage = ancestors.FirstOrDefault(x => x is HomePage) as HomePage;
            return startPage == null ? ContentReference.StartPage : startPage.ContentLink;
        }

        public static bool IsEqual(this AddressModel address,
           AddressModel compareAddressViewModel)
        {
            return address.FirstName == compareAddressViewModel.FirstName &&
                   address.LastName == compareAddressViewModel.LastName &&
                   address.Line1 == compareAddressViewModel.Line1 &&
                   address.Line2 == compareAddressViewModel.Line2 &&
                   address.Organization == compareAddressViewModel.Organization &&
                   address.PostalCode == compareAddressViewModel.PostalCode &&
                   address.City == compareAddressViewModel.City &&
                   address.CountryCode == compareAddressViewModel.CountryCode &&
                   address.CountryRegion.Region == compareAddressViewModel.CountryRegion.Region;
        }

        public static List<string> TagString(this LocationItemPage locationList) => locationList.Categories.Select(cai => _contentRepository.Value.Get<StandardCategory>(cai).Name).ToList();

        public static ContactViewModel GetCurrentContactViewModel(this ICustomerService customerService)
        {
            var currentContact = customerService.GetCurrentContact();
            return currentContact?.Contact != null ? new ContactViewModel(currentContact) : new ContactViewModel();
        }

        public static ContactViewModel GetContactViewModelById(this ICustomerService customerService, string id) => new ContactViewModel(customerService.GetContactById(id));

        public static List<ContactViewModel> GetContactViewModelsForOrganization(this ICustomerService customerService, FoundationOrganization organization = null)
        {
            if (organization == null)
            {
                organization = GetCurrentOrganization(customerService);
            }

            if (organization == null)
            {
                return new List<ContactViewModel>();
            }

            var organizationUsers = customerService.GetContactsForOrganization(organization);

            if (organization.SubOrganizations.Count > 0)
            {
                foreach (var subOrg in organization.SubOrganizations)
                {
                    var contacts = customerService.GetContactsForOrganization(subOrg);
                    organizationUsers.AddRange(contacts);
                }
            }

            return organizationUsers.Select(user => new ContactViewModel(user)).ToList();
        }

        public static IEnumerable<T> FindPagesRecursively<T>(this IContentLoader contentLoader, PageReference pageLink) where T : PageData
        {
            foreach (var child in contentLoader.GetChildren<T>(pageLink))
            {
                yield return child;
            }

            foreach (var folder in contentLoader.GetChildren<FolderPage>(pageLink))
            {
                foreach (var nestedChild in contentLoader.FindPagesRecursively<T>(folder.PageLink))
                {
                    yield return nestedChild;
                }
            }
        }

        public static bool IsVirtualVariant(this ILineItem lineItem)
        {
            var entry = lineItem.GetEntryContent<EntryContentBase>() as GenericVariant;
            return entry != null && entry.VirtualProductMode != null && !string.IsNullOrWhiteSpace(entry.VirtualProductMode) && !entry.VirtualProductMode.Equals("None");
        }

        public static void InstallDefaultContent()
        {
            if (_siteDefinitionRepository.Value.List().Any() || Type.GetType("Foundation.Features.Setup.SetupController, Foundation") != null)
            {
                return;
            }

            var siteDefinition = new SiteDefinition
            {
                Name = "foundation",
                SiteUrl = new Uri($"http://{_installService.Value.FoundationConfiguration.ApplicationName}"),
                Hosts = new List<HostDefinition>()
                    {
                        new HostDefinition { Name = "*", Type = HostDefinitionType.Undefined }
                    }
            };

            if (!_installService.Value.FoundationConfiguration.SitePublicDomain.IsNullOrEmpty() &&
                !_installService.Value.FoundationConfiguration.ApplicationName.Equals(_installService.Value.FoundationConfiguration.SitePublicDomain))
            {
                siteDefinition.Hosts.Add(new HostDefinition
                {
                    Name = _installService.Value.FoundationConfiguration.SitePublicDomain,
                    Type = HostDefinitionType.Primary
                });
            }
            else
            {
                siteDefinition.Hosts.Add(new HostDefinition
                {
                    Name = _installService.Value.FoundationConfiguration.ApplicationName,
                    Type = HostDefinitionType.Primary
                });
            }

            CreateSite(new FileStream(HostingEnvironment.MapPath("~/App_Data/foundation.episerverdata"), FileMode.Open),
                siteDefinition,
                ContentReference.RootPage);

            ServiceLocator.Current.GetInstance<ISettingsService>().UpdateSettings();

            CreateCatalog(new FileStream(HostingEnvironment.MapPath("~/App_Data/foundation_fashion.zip"), FileMode.Open),
                Path.Combine(HostingEnvironment.MapPath("~/App_Data/foundation_fashion.zip")));

            var searchManager = new SearchManager(Mediachase.Commerce.Core.AppContext.Current.ApplicationName);
            searchManager.BuildIndex(true);

            var config = Configuration.GetConfiguration();
            if (!config.ServiceUrl.Equals("https://es-us-api01.episerver.com/9IKGqgMZaTD9KP4Op3ygsVB6JeJzR0N6") && !config.DefaultIndex.Equals("episerverab_index55794"))
            {
                RunIndexJob(ServiceLocator.Current.GetInstance<IScheduledJobExecutor>(),
                    ServiceLocator.Current.GetInstance<IScheduledJobRepository>(), new Guid("8EB257F9-FF22-40EC-9958-C1C5BA8C2A53"));
            }
        }

        private static void CreateSite(Stream stream, SiteDefinition siteDefinition, ContentReference startPage)
        {
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.EventedIndexingEnabled = false;
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.ScheduledPageQueueEnabled = false;
            ImportEpiserverContent(stream, startPage, ServiceLocator.Current.GetInstance<IDataImporter>(), siteDefinition);
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.EventedIndexingEnabled = true;
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.ScheduledPageQueueEnabled = true;
        }

        private static void CreateCatalog(Stream file, string fileName)
        {
            if (file == null || fileName.IsNullOrEmpty())
            {
                throw new Exception("File is required");
            }
            var name = fileName.Substring(fileName.LastIndexOf("\\") == 0 ? 0 : fileName.LastIndexOf("\\") + 1);
            var path = HostingEnvironment.MapPath("~/App_Data/Catalog");
            var zipFile = Path.Combine(path, name);
            var zipDirectory = new DirectoryInfo(Path.Combine(path, name.Replace(".zip", "")));
            if (zipDirectory.Exists)
            {
                zipDirectory.Delete(true);
            }

            zipDirectory.Create();

            var zipInputStream = new ZipFile(file)
            {
                IsStreamOwner = false
            };
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

            var catalogFolder = _contentRepository.Value.GetChildren<ContentFolder>(ContentReference.GlobalBlockFolder)
                .FirstOrDefault(_ => _.Name.Equals("Catalogs"));

            if (catalogFolder == null)
            {
                catalogFolder = _contentRepository.Value.GetDefault<ContentFolder>(ContentReference.GlobalBlockFolder);
                catalogFolder.Name = "Catalogs";
                _contentRepository.Value.Save(catalogFolder, EPiServer.DataAccess.SaveAction.Publish, EPiServer.Security.AccessLevel.NoAccess);
            }

            EPiServer.Find.Cms.EventedIndexingSettings.Instance.EventedIndexingEnabled = false;
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.ScheduledPageQueueEnabled = false;
            ImportEpiserverContent(assests.OpenRead(), catalogFolder.ContentLink, ServiceLocator.Current.GetInstance<IDataImporter>());
            try
            {
                var catalogImportExport = new CatalogImportExport()
                {
                    IsModelsAvailable = true
                };
                catalogImportExport.Import(catalogXml.OpenRead(), true);
            }
            catch (Exception exception)
            {
                LogManager.GetLogger().Error(exception.Message, exception);
            }

            EPiServer.Find.Cms.EventedIndexingSettings.Instance.EventedIndexingEnabled = true;
            EPiServer.Find.Cms.EventedIndexingSettings.Instance.ScheduledPageQueueEnabled = true;
        }

        public static bool ImportEpiserverContent(Stream stream,
            ContentReference destinationRoot,
            IDataImporter importer,
            SiteDefinition siteDefinition = null)
        {
            var success = false;
            try
            {
                var log = importer.Import(stream, destinationRoot, new ImportOptions
                {
                    KeepIdentity = true,
                });

                var status = importer.Status;
                if (status == null)
                {
                    return false;
                }

                UpdateLanguageBranches(status);
                if (siteDefinition != null && !ContentReference.IsNullOrEmpty(status.ImportedRoot))
                {
                    siteDefinition.StartPage = status.ImportedRoot;
                    _siteDefinitionRepository.Value.Save(siteDefinition);
                    SiteDefinition.Current = siteDefinition;
                    success = true;
                }
            }
            catch (Exception exception)
            {
                LogManager.GetLogger().Error(exception.Message, exception);
                success = false;
            }

            return success;
        }

        private static void UpdateLanguageBranches(IImportStatus status)
        {
            var languageBranchRepository = ServiceLocator.Current.GetInstance<ILanguageBranchRepository>();

            if (status.ContentLanguages == null)
            {
                return;
            }

            foreach (var languageId in status.ContentLanguages)
            {
                var languageBranch = languageBranchRepository.Load(languageId);

                if (languageBranch == null)
                {
                    languageBranch = new LanguageBranch(languageId);
                    languageBranchRepository.Save(languageBranch);
                }
                else if (!languageBranch.Enabled)
                {
                    languageBranch = languageBranch.CreateWritableClone();
                    languageBranch.Enabled = true;
                    languageBranchRepository.Save(languageBranch);
                }
            }
        }

        private static void RunIndexJob(IScheduledJobExecutor scheduledJobExecutor, IScheduledJobRepository scheduledJobRepository, Guid jobId)
        {
            var job = scheduledJobRepository.Get(jobId);
            if (job == null)
            {
                return;
            }

            scheduledJobExecutor.StartAsync(job, new JobExecutionOptions { Trigger = ScheduledJobTrigger.User });
        }

        private static FoundationOrganization GetCurrentOrganization(ICustomerService customerService)
        {
            var contact = customerService.GetCurrentContact();
            if (contact != null)
            {
                return contact.FoundationOrganization;
            }

            return null;
        }
    }
}