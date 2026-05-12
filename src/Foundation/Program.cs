using Microsoft.AspNetCore.Hosting;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using Serilog;
using System.IO;
using System.Text.RegularExpressions;

namespace Foundation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            var isDevelopment = environment == Environments.Development;

            // On DXP (Linux Azure App Service), EPiServerDB is injected as SQLCONNSTR_EPiServerDB
            // but EcfSqlConnection is not injected separately (single-database setup).
            // Mediachase.Data.Provider.SqlDataProvider.Initialize reads from
            // System.Configuration.ConfigurationManager (not IConfiguration), which maps
            // SQLCONNSTR_* env vars to ConfigurationManager.ConnectionStrings[name].
            // Mirror EPiServerDB → EcfSqlConnection early so both ConfigurationManager and
            // IConfiguration see EcfSqlConnection before any host configuration runs.
            if (!isDevelopment)
            {
                var epiDb = Environment.GetEnvironmentVariable("SQLCONNSTR_EPiServerDB")
                         ?? Environment.GetEnvironmentVariable("CUSTOMCONNSTR_EPiServerDB")
                         ?? Environment.GetEnvironmentVariable("SQLAZURECONNSTR_EPiServerDB");
                var ecfExists = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("SQLCONNSTR_EcfSqlConnection"))
                             || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("CUSTOMCONNSTR_EcfSqlConnection"));
                if (!string.IsNullOrEmpty(epiDb) && !ecfExists)
                    Environment.SetEnvironmentVariable("SQLCONNSTR_EcfSqlConnection", epiDb);

                // v2 bacpac databases do not include Commerce meta-tables (mcmd_*).
                // Commerce initialization crashes in SqlSerializer.DeserializeInternal() when
                // mcmd_MetaFieldType doesn't exist (SqlContext.GetTable returns null → TableConfig.Primary
                // is null → SelectCommandBuilder.WriteQuery() NullRef → 503).
                // Run the Commerce SQL init script before the host starts to create all Commerce tables.
                if (!string.IsNullOrEmpty(epiDb))
                {
                    EnsureCmsTypes(epiDb);
                    EnsureCommerceSchema(epiDb);
                    EnsureBFMetaClassPKs(epiDb);
                    EnsureBFMetaFieldTypes(epiDb);
                }
            }

            if (isDevelopment)
            {
                Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.File("App_Data/log.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();
            }

            CreateHostBuilder(args, isDevelopment).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args, bool isDevelopment)
        {
            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureCmsDefaults()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });

            if (isDevelopment)
                builder = builder.UseSerilog();

            return builder;
        }

        private static void EnsureCmsTypes(string connectionString)
        {
            // ApplicationHostTable and ApplicationUrlFormatTable are CMS 13 user-defined table types
            // (TVPs) used by ApplicationDB.SaveAsync when saving an Application. On DXP the bacpac
            // import creates these types in the [david_cms13-upgrade.CmsUser] schema (the local SQL
            // user's default schema) instead of [dbo]. The CMS inline SQL references them without a
            // schema prefix; DXP Azure SQL users have [dbo] as their default schema so the lookup
            // fails with error 351 "Cannot find data type ApplicationHostTable".
            // Creating them in [dbo] here fixes the lookup on Azure SQL without affecting local dev
            // (where the CmsUser-schema versions are found via the user's default schema).
            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                (string Name, string Ddl)[] types =
                [
                    ("ApplicationHostTable", @"CREATE TYPE [dbo].[ApplicationHostTable] AS TABLE (
                        [Authority]           NVARCHAR(MAX) NULL,
                        [Type]                INT           NULL,
                        [Locale]              NVARCHAR(255) NULL,
                        [UseSecureConnection] BIT           NULL)"),
                    ("ApplicationUrlFormatTable", @"CREATE TYPE [dbo].[ApplicationUrlFormatTable] AS TABLE (
                        [fkContentTypeGUID] UNIQUEIDENTIFIER NULL,
                        [Base]              NVARCHAR(50)     NULL,
                        [Type]              INT              NULL,
                        [Format]            NVARCHAR(MAX)    NULL)"),
                ];

                foreach (var (name, ddl) in types)
                {
                    using var check = conn.CreateCommand();
                    check.CommandText = @"SELECT COUNT(1) FROM sys.types t
                        JOIN sys.schemas s ON t.schema_id = s.schema_id
                        WHERE t.name = @n AND s.name = 'dbo'";
                    check.Parameters.AddWithValue("@n", name);
                    if ((int)check.ExecuteScalar() == 0)
                    {
                        using var create = conn.CreateCommand();
                        create.CommandText = ddl;
                        create.ExecuteNonQuery();
                        Console.WriteLine($"[PreInit] Created dbo.{name}.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PreInit] CMS types init failed: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private static void EnsureBFMetaFieldTypes(string connectionString)
        {
            // The DXP combined bacpac includes mcmd_MetaFieldType rows 1–25 (standard Commerce types
            // created by EPiServer.Commerce.Core.sql). Six custom BF types (IDs 26–31) are only added
            // by Commerce during its first-run metadata initialization against a live DB with Customer
            // data; they are absent from a bacpac built by merging SQL scripts + SELECT INTO.
            //
            // When BF loads a MetaClass whose fields reference these types — e.g. Contact.CustomerGroup
            // (TypeName='ContactGroup'), Contact.ShowInDemoUserMenu (TypeName='DemoUserMenu'),
            // Organization.OrganizationType/BusinessCategory, Address.AddressType, CreditCard.CardType —
            // the type lookup returns null and MetaClass.GetJoinedTableList() throws NullReferenceException.
            // This crashes CustomerContext.GetContactByUserId() on every request, including Commerce Shell
            // controllers (DashboardController, MarketingController, CatalogController).
            //
            // Fix: insert the six missing rows so BF can resolve all field types.
            // Uses INSERT without IDENTITY_INSERT because BF looks up types by Name, not by ID.
            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                (string Name, string FriendlyName, string? XSAttributes)[] types =
                [
                    ("OrganizationType", "OrganizationType", null),
                    ("BusinessCategory",  "BusinessCategory",  null),
                    ("AddressType",       "AddressType",       "<?xml version=\"1.0\" encoding=\"utf-8\"?>\n<AttributeCollection>\n  <Attr>\n    <Name>MultiValue</Name>\n    <Type>System.Boolean, System.Private.CoreLib</Type>\n    <Value><boolean>true</boolean></Value>\n  </Attr>\n</AttributeCollection>"),
                    ("ContactGroup",      "ContactGroup",      null),
                    ("CreditCardType",    "CreditCardType",    null),
                    ("DemoUserMenu",      "Show in Demo User Menu", null),
                ];

                foreach (var (name, friendly, xsAttr) in types)
                {
                    using var check = conn.CreateCommand();
                    check.CommandText = "SELECT COUNT(1) FROM mcmd_MetaFieldType WHERE Name = @n";
                    check.Parameters.AddWithValue("@n", name);
                    if ((int)check.ExecuteScalar() == 0)
                    {
                        using var insert = conn.CreateCommand();
                        insert.CommandText = @"INSERT INTO mcmd_MetaFieldType (Name, FriendlyName, McDataType, XSViews, XSAttributes, Owner, AccessLevel)
                            VALUES (@name, @friendly, 8, NULL, @xsAttr, 'System', 1)";
                        insert.Parameters.AddWithValue("@name", name);
                        insert.Parameters.AddWithValue("@friendly", friendly);
                        insert.Parameters.AddWithValue("@xsAttr", (object?)xsAttr ?? DBNull.Value);
                        insert.ExecuteNonQuery();
                        Console.WriteLine($"[PreInit] Inserted mcmd_MetaFieldType: {name}.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PreInit] BF MetaFieldType init failed: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private static void EnsureBFMetaClassPKs(string connectionString)
        {
            // The cls_* tables were copied to DXP via SELECT INTO (no PKs/constraints).
            // Commerce requires proper PK constraints on cls_* tables for correct BF operation.
            // Fix: add PRIMARY KEY CLUSTERED constraints discovered from the local Commerce DB.
            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();

                using (var checkCmd = conn.CreateCommand())
                {
                    checkCmd.CommandText = @"SELECT COUNT(1) FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                        WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = 'cls_Contact'";
                    if ((int)checkCmd.ExecuteScalar() > 0)
                    {
                        Console.WriteLine("[PreInit] BF MetaClass PKs already present, skipping.");
                        return;
                    }
                }

                Console.WriteLine("[PreInit] Adding PKs to cls_* tables for BF MetaClass...");

                (string Table, string Column)[] pks =
                [
                    ("cls_Address",                "AddressId"),
                    ("cls_Budget",                 "BudgetId"),
                    ("cls_Contact",                "ContactId"),
                    ("cls_ContactNote",            "ContactNoteId"),
                    ("cls_CreditCard",             "CreditCardId"),
                    ("cls_GiftCard",               "GiftCardId"),
                    ("cls_Organization",           "OrganizationId"),
                    ("cls_RecentReferenceHistory", "RecentReferenceHistoryId"),
                ];

                foreach (var (table, column) in pks)
                {
                    try
                    {
                        using var cmd = conn.CreateCommand();
                        cmd.CommandText = $@"
                            IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{table}')
                            AND NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS
                                            WHERE CONSTRAINT_TYPE = 'PRIMARY KEY' AND TABLE_NAME = '{table}')
                            BEGIN
                                ALTER TABLE [dbo].[{table}]
                                ADD CONSTRAINT [PK_{table}] PRIMARY KEY CLUSTERED ([{column}]);
                            END";
                        cmd.ExecuteNonQuery();
                        Console.WriteLine($"[PreInit] PK added/verified: {table}.{column}.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[PreInit] Failed to add PK to {table}: {ex.Message.Split('\n')[0]}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PreInit] BF MetaClass PK init failed: {ex.GetType().Name}: {ex.Message}");
            }
        }

        private static void EnsureCommerceSchema(string connectionString)
        {
            try
            {
                using var conn = new SqlConnection(connectionString);
                conn.Open();
                using (var checkCmd = conn.CreateCommand())
                {
                    // ReportingDates is populated by the LAST batch in EPiServer.Commerce.Core.sql
                    // (EXEC ecf_GenerateReportingDates at line 26860). Rows in this table prove the
                    // full script ran to completion. Earlier sentinels like GetContentSchemaVersionNumber
                    // (line 10653) are unreliable — a startup killed mid-script would leave that proc
                    // existing while later objects are still missing.
                    checkCmd.CommandText = @"SELECT CASE
                        WHEN EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = 'ReportingDates')
                         AND (SELECT COUNT(*) FROM [dbo].[ReportingDates]) > 0
                        THEN 1 ELSE 0 END";
                    var isComplete = (int)checkCmd.ExecuteScalar() == 1;
                    if (isComplete) { Console.WriteLine("[PreInit] Commerce schema already installed, skipping."); return; }
                }
                Console.WriteLine("[PreInit] Commerce meta-tables not found. Running schema initialization...");
                var sqlPath = Path.Combine(AppContext.BaseDirectory, "App_Data", "EPiServer.Commerce.Core.sql");
                if (!File.Exists(sqlPath)) { Console.WriteLine($"[PreInit] SQL script not found at {sqlPath}."); return; }
                var sql = File.ReadAllText(sqlPath);
                var batches = Regex.Split(sql, @"^\s*GO\s*(?:\d+)?\s*(?:--[^\r\n]*)?\r?$",
                    RegexOptions.Multiline);
                int ok = 0, skipped = 0, failed = 0;
                foreach (var batch in batches)
                {
                    var trimmed = batch.Trim();
                    if (string.IsNullOrWhiteSpace(trimmed)) continue;
                    using var cmd = conn.CreateCommand();
                    cmd.CommandText = trimmed;
                    cmd.CommandTimeout = 120;
                    try { cmd.ExecuteNonQuery(); ok++; }
                    catch (SqlException ex) when (ex.Number == 2714 || ex.Number == 1913 || ex.Number == 2715)
                    { skipped++; } // Object or type already exists
                    catch (Exception ex)
                    { failed++; Console.WriteLine($"[PreInit] SQL batch error ({ex.GetType().Name}): {ex.Message.Split('\n')[0]}"); }
                }
                Console.WriteLine($"[PreInit] Commerce schema init: {ok} OK, {skipped} skipped, {failed} failed.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[PreInit] Commerce schema init failed: {ex.GetType().Name}: {ex.Message}");
            }
        }
    }
}
