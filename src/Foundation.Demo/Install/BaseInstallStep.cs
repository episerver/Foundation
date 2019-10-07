using EPiServer;
using EPiServer.Core;
using EPiServer.Enterprise;
using EPiServer.Logging;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Shared;
using Mediachase.Data.Provider;
using Mediachase.MetaDataPlus.Configurator;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Linq;

namespace Foundation.Demo.Install
{
    public abstract class BaseInstallStep : IInstallStep
    {
        protected IConnectionStringHandler ConnectionStringHandler { get; }
        protected IContentRepository ContentRepository { get; }
        protected CustomerContext CustomerContext { get; }
        protected IDataImporter DataImporter { get; }
        protected ReferenceConverter ReferenceConverter { get; }
        protected IMarketService MarketService { get; }
        protected ILogger Logger { get; }



        protected BaseInstallStep(IContentRepository contentRepository,
            IDataImporter dataImporter,
            ReferenceConverter referenceConverter,
            IMarketService marketService)
        {
            Logger = LogManager.GetLogger(GetType());
            CustomerContext = CustomerContext.Current;
            ContentRepository = contentRepository;
            DataImporter = dataImporter;
            ReferenceConverter = referenceConverter;
            MarketService = marketService;
        }

        public abstract int Order { get; }
        public abstract string Description { get; }
        public string Name => GetType().Name;

        public bool Execute(IProgressMessenger progressMessenger)
        {
            progressMessenger.AddProgressMessageText($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - Started step {GetType().Name}", false, 0);
            var name = GetType().Name;
            try
            {
                ExecuteInternal(progressMessenger);
            }
            catch (Exception ex)
            {
                progressMessenger.AddProgressMessageText($"Error executing step {name} with message\n {ex.Message}.\nPlease see log for more details.", true, 0);
                Logger.Error($"Error executing step {name}", ex);
                throw;
            }


            progressMessenger.AddProgressMessageText($"{DateTime.Now.ToString(CultureInfo.InvariantCulture)} - Ended {GetType().Name}", false, 0);
            progressMessenger.AddProgressMessageText("Step completed successfully", false, 0);
            return true;
        }

        protected abstract void ExecuteInternal(IProgressMessenger progressMessenger);

        protected virtual void TryAddMetaField(Mediachase.MetaDataPlus.MetaDataContext context,
            Mediachase.MetaDataPlus.Configurator.MetaClass metaClass,
            string name,
            MetaDataType metaDataType,
            int length)
        {
            var metaField = Mediachase.MetaDataPlus.Configurator.MetaField.Load(context, name) ?? Mediachase.MetaDataPlus.Configurator.MetaField.Create(
                                context: context,
                                metaNamespace: metaClass.Namespace,
                                name: name,
                                friendlyName: name,
                                description: name,
                                dataType: metaDataType,
                                length: length,
                                allowNulls: true,
                                multiLanguageValue: false,
                                allowSearch: false,
                                isEncrypted: false);

            if (metaClass.MetaFields.All(x => x.Id != metaField.Id))
            {
                metaClass.AddField(metaField);
            }
        }

        protected virtual void ImportEpiserverData(Stream stream)
        {
            var destinationRoot = ContentReference.GlobalBlockFolder;
            var keys = new List<string>();
            foreach (DictionaryEntry entry in HttpRuntime.Cache)
            {
                keys.Add((string)entry.Key);
            }
            foreach (var key in keys)
            {
                HttpRuntime.Cache.Remove(key);
            }

            var options = new ImportOptions { KeepIdentity = true };

            var log = DataImporter.Import(stream, destinationRoot, options);

            if (log.Errors.Any())
            {
                throw new Exception("Content could not be imported. " + GetStatus(log));
            }
        }

        protected virtual IEnumerable<XElement> GetXElements(Stream stream, XName elementName)
        {
            if (stream.Position != 0)
            {
                stream.Position = 0;
            }

            using (var reader = XmlReader.Create(stream))
            {
                reader.MoveToContent();
                // Parse the file and return each of the child_node
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element && reader.Name == elementName)
                    {
                        var element = XNode.ReadFrom(reader) as XElement;
                        if (element != null)
                        {
                            yield return element;
                        }

                    }
                }
            }
        }

        private string GetStatus(ITransferLog log)
        {
            var logMessage = new StringBuilder();
            var lineBreak = "<br>";

            if (log.Errors.Any())
            {
                foreach (var err in log.Errors)
                {
                    logMessage.Append(err).Append(lineBreak);
                }
            }

            if (log.Warnings.Any())
            {
                foreach (var err in log.Warnings)
                {
                    logMessage.Append(err).Append(lineBreak);
                }
            }
            return logMessage.ToString();
        }
    }
}
