using EPiServer;
using EPiServer.Logging;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Customers;
using Mediachase.Commerce.Markets;
using Mediachase.Commerce.Shared;
using Mediachase.Data.Provider;
using Mediachase.MetaDataPlus.Configurator;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace Foundation.Commerce.Install
{
    public abstract class BaseInstallStep : IInstallStep
    {
        protected IConnectionStringHandler ConnectionStringHandler { get; }
        protected IContentRepository ContentRepository { get; }
        protected CustomerContext CustomerContext { get; }
        protected ReferenceConverter ReferenceConverter { get; }
        protected IMarketService MarketService { get; }
        protected ILogger Logger { get; }

        protected BaseInstallStep(IContentRepository contentRepository,
            ReferenceConverter referenceConverter,
            IMarketService marketService)
        {
            Logger = LogManager.GetLogger(GetType());
            CustomerContext = CustomerContext.Current;
            ContentRepository = contentRepository;
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
            MetaClass metaClass,
            string name,
            MetaDataType metaDataType,
            int length)
        {
            var metaField = MetaField.Load(context, name) ?? MetaField.Create(
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
    }
}
