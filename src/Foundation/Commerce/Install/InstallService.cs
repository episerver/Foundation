using EPiServer.Logging;
using EPiServer.ServiceLocation;
using Mediachase.Commerce.Catalog.ImportExport;
using Mediachase.Data.Provider;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace Foundation.Commerce.Install
{
    public class InstallService : IInstallService
    {
        private readonly ILogger _logger = LogManager.GetLogger(typeof(InstallService));
        private readonly IConnectionStringHandler _connectionStringHandler;
        private FoundationConfiguration _foundationConfiguration;
        private InstallProgressMessenger _progressMessenger;
        private IEnumerable<IInstallStep> _installSteps;

        public InstallService(IConnectionStringHandler connectionStringHandler) => _connectionStringHandler = connectionStringHandler;

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
