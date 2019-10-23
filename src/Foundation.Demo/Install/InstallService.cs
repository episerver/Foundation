using EPiServer.Core;
using EPiServer.DataAbstraction;
using EPiServer.Enterprise;
using EPiServer.Logging;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Mediachase.Commerce.Catalog.ImportExport;
using Mediachase.Data.Provider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Foundation.Demo.Install
{
    public class InstallService : IInstallService
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(InstallService));
        private readonly ServiceAccessor<IDataImporter> _dataImporter;
        private readonly ILanguageBranchRepository _languageBranchRepository;
        private readonly ISiteDefinitionRepository _siteDefinitionRepository;
        private readonly ContentExportProcessor _contentExportProcessor;
        private readonly IConnectionStringHandler _connectionStringHandler;
        private FoundationConfiguration _foundationConfiguration;
        private InstallProgressMessenger _progressMessenger;
        private IEnumerable<IInstallStep> _installSteps;

        public InstallService(ServiceAccessor<IDataImporter> dataImporter,
            ILanguageBranchRepository languageBranchRepository,
            ISiteDefinitionRepository siteDefinitionRepository,
            ContentExportProcessor contentExportProcessor,
            IConnectionStringHandler connectionStringHandler)

        {
            _dataImporter = dataImporter;
            _languageBranchRepository = languageBranchRepository;
            _siteDefinitionRepository = siteDefinitionRepository;
            _contentExportProcessor = contentExportProcessor;
            _connectionStringHandler = connectionStringHandler;
        }

        public IEnumerable<IInstallStep> InstallSteps
        {
            get => _installSteps ?? (_installSteps = ServiceLocator.Current.GetAllInstances<IInstallStep>());
            set => _installSteps = value;
        }

        public InstallProgressMessenger ProgressMessenger
        {
            get => _progressMessenger ?? (_progressMessenger = new InstallProgressMessenger());
            set => _progressMessenger = value;
        }

        public FoundationConfiguration FoundationConfiguration => _foundationConfiguration ?? (_foundationConfiguration = GetFoundationConfiguration());

        public Stream ExportCatalog(string name)
        {
            try
            {
                var stream = new MemoryStream();
                new CatalogImportExport
                {
                    IsModelsAvailable = true
                }.Export(name, stream, "");
                stream.Position = 0;
                return stream;
            }
            catch (Exception exception)
            {
                _logger.Error(exception.Message, exception);
                ProgressMessenger.AddProgressMessageText(exception.Message, true, 100);
            }

            return null;

        }

        public Stream ExportEpiserverContent(ContentReference root, ContentExport contentExport)
        {
            try
            {
                return _contentExportProcessor.ExportContent(contentExport, root);
            }
            catch (Exception exception)
            {
                ProgressMessenger.AddProgressMessageText(exception.Message, true, 100);
                _logger.Error(exception.Message, exception);
            }

            return null;
        }

        public bool ImportCatalog(Stream stream)
        {
            bool success;
            try
            {
                var catalogImportExport = new CatalogImportExport()
                {
                    IsModelsAvailable = true
                };
                catalogImportExport.ImportExportProgressMessage += (s, e) =>
                {
                    ProgressMessenger.AddProgressMessageText(e.Message, false, 0);
                };

                catalogImportExport.Import(stream, true);
                success = true;
            }
            catch (Exception exception)
            {
                ProgressMessenger.AddProgressMessageText(exception.Message, true, 100);
                _logger.Error(exception.Message, exception);
                success = false;
            }

            return success;


        }

        public bool ImportEpiserverContent(Stream stream, ContentReference destinationRoot, SiteDefinition siteDefinition = null)
        {
            var success = false;
            try
            {
                var importer = _dataImporter();
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
                    _siteDefinitionRepository.Save(siteDefinition);
                    SiteDefinition.Current = siteDefinition;
                    success = true;
                }
            }
            catch (Exception exception)
            {
                ProgressMessenger.AddProgressMessageText(exception.Message, true, 100);
                _logger.Error(exception.Message, exception);
                success = false;
            }

            return success;
        }

        public void RunInstallSteps()
        {
            foreach (var step in InstallSteps.OrderBy(x => x.Order))
            {
                var next = InstallStep(step);
                if (!next)
                {
                    return;
                }
            }
            UpdateFoundationConfiguration();
        }

        public bool ShouldInstall() => !FoundationConfiguration?.IsInstalled ?? false;

        private void UpdateFoundationConfiguration()
        {
            using (var connection = new SqlConnection(_connectionStringHandler.Commerce.ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "FoundationConfiguration_SetInstalled",
                };
                command.ExecuteNonQuery();
            }
        }

        private bool InstallStep(IInstallStep installStep)
        {
            ProgressMessenger.AddProgressMessageText("Starting migration step: " + installStep.Name, false, 0);
            var success = installStep.Execute(ProgressMessenger);
            ProgressMessenger.AddProgressMessageText("Completed migration step: " + installStep.Name, false, 0);
            return success;
        }

        private void UpdateLanguageBranches(IImportStatus status)
        {
            if (status.ContentLanguages == null)
            {
                return;
            }

            foreach (var languageId in status.ContentLanguages)
            {
                var languageBranch = _languageBranchRepository.Load(languageId);

                if (languageBranch == null)
                {
                    languageBranch = new LanguageBranch(languageId);
                    _languageBranchRepository.Save(languageBranch);
                }
                else if (!languageBranch.Enabled)
                {
                    languageBranch = languageBranch.CreateWritableClone();
                    languageBranch.Enabled = true;
                    _languageBranchRepository.Save(languageBranch);
                }
            }
        }

        private FoundationConfiguration GetFoundationConfiguration()
        {
            using (var connection = new SqlConnection(_connectionStringHandler.Commerce.ConnectionString))
            {
                connection.Open();
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "FoundationConfiguration_List",
                };
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        return new FoundationConfiguration
                        {
                            ApplicationName = reader["AppName"].ToString(),
                            CommerceMangerDomain = reader["CMHostname"].ToString(),
                            IsInstalled = Convert.ToBoolean(reader["IsInstalled"]),
                            SitePublicDomain = reader["FoundationHostname"].ToString()
                        };
                    }
                }
            }

            return null;
        }
    }
}
