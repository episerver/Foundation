using EPiServer.Core;
using EPiServer.Core.Transfer;
using EPiServer.DataAbstraction;
using EPiServer.Enterprise;
using EPiServer.Enterprise.Internal;
using EPiServer.Personalization.VisitorGroups;
using EPiServer.ServiceLocation;
using EPiServer.Web;
using Foundation.Cms.Extensions;
using System;
using System.Collections.Generic;
using System.IO;

namespace Foundation.Demo.Install
{
    [Flags]
    public enum ContentExport
    {
        ExportContentTypes = 1,
        ExportFrames = 2,
        ExportTabDefinitions = 4,
        ExportDynamicPropertyDefinitions = 8,
        ExportCategories = 16,
        ExportPages = 32,
        ExportContentTypeDependencies = 64,
        ExportVisitorGroups = 128,
        ExportPropertySettings = 256
    }

    public class ContentExportProcessor
    {
        private readonly ServiceAccessor<IDataExporter> _dataExporterAccessor;
        private readonly IPropertyDefinitionRepository _propertyDefinitionRepository;
        private readonly IContentTypeRepository _contentTypeRepository;
        private readonly ITabDefinitionRepository _tabDefinitionRepository;
        private readonly ISiteDefinitionRepository _siteDefinitionRepository;
        private readonly CategoryRepository _categoryRepository;
        private readonly IVisitorGroupRepository _visitorGroupRepository;
        private readonly ServiceAccessor<IFrameRepository> _frameRepository;

        public ContentExportProcessor(IPropertyDefinitionRepository propertyDefinitionRepository,
            IContentTypeRepository contentTypeRepository,
            ITabDefinitionRepository tabDefinitionRepository,
            ISiteDefinitionRepository siteDefinitionRepository,
            CategoryRepository categoryRepository,
            IVisitorGroupRepository visitorGroupRepository,
            ServiceAccessor<IFrameRepository> frameRepository,
            ServiceAccessor<IDataExporter> dataExporterAccessor)
        {
            _contentTypeRepository = contentTypeRepository;
            _propertyDefinitionRepository = propertyDefinitionRepository;
            _tabDefinitionRepository = tabDefinitionRepository;
            _siteDefinitionRepository = siteDefinitionRepository;
            _categoryRepository = categoryRepository;
            _visitorGroupRepository = visitorGroupRepository;
            _frameRepository = frameRepository;
            _dataExporterAccessor = dataExporterAccessor;
        }

        public Stream ExportContent(ContentExport contentExport, ContentReference root)
        {
            var exporter = _dataExporterAccessor();
            var exportedContentTypes = new HashSet<int>();
            var sources = new List<ExportSource>();

            if ((contentExport & ContentExport.ExportContentTypes) == ContentExport.ExportContentTypes)
            {
                _contentTypeRepository.List().ForEach(x =>
                {
                    exporter.AddContentType(x);
                    exportedContentTypes.Add(x.ID);
                });
            }

            if ((contentExport & ContentExport.ExportFrames) == ContentExport.ExportFrames)
            {
                _frameRepository().List().ForEach(exporter.AddFrame);
            }

            if ((contentExport & ContentExport.ExportTabDefinitions) == ContentExport.ExportTabDefinitions)
            {
                _tabDefinitionRepository.List().ForEach(exporter.AddTabDefinition);
            }

            if ((contentExport & ContentExport.ExportDynamicPropertyDefinitions) == ContentExport.ExportDynamicPropertyDefinitions)
            {
                _propertyDefinitionRepository.ListDynamic().ForEach(exporter.AddDynamicProperty);
            }

            if ((contentExport & ContentExport.ExportCategories) == ContentExport.ExportCategories)
            {
                ExportCategories(exporter, _categoryRepository.GetRoot());
            }

            if ((contentExport & ContentExport.ExportPages) == ContentExport.ExportPages)
            {
                sources.Add(new ExportSource(root, ExportSource.RecursiveLevelInfinity));
            }

            if ((contentExport & ContentExport.ExportVisitorGroups) == ContentExport.ExportVisitorGroups)
            {
                _visitorGroupRepository.List().ForEach(exporter.AddVisitorGroup);
            }

            var options = new ExportOptions
            {
                IncludeReferencedContentTypes =
                    (contentExport & ContentExport.ExportContentTypeDependencies) == ContentExport.ExportContentTypeDependencies,
                ExportPropertySettings =
                    (contentExport & ContentExport.ExportPropertySettings) == ContentExport.ExportPropertySettings,
                ExcludeFiles = false,
                AutoCloseStream = false
            };

            var stream = new MemoryStream();
            exporter.Export(stream, sources, options);
            ((DefaultDataExporter)exporter)?.Close();
            stream.Position = 0;
            return stream;
        }

        private void ExportCategories(IDataExporter exporter, Category cat)
        {
            foreach (var category in cat.Categories)
            {
                exporter.AddCategory(category);
                if (category.Categories.Count > 0)
                {
                    ExportCategories(exporter, category);
                }
            }
        }

        public Stream ExportEpiserverVisitorGroup(List<VisitorGroup> visitorGroups)
        {
            var exporter = _dataExporterAccessor();
            var stream = new MemoryStream();
            var sources = new List<ExportSource>();

            var options = new ExportOptions
            {
                ExcludeFiles = false,
                AutoCloseStream = false
            };

            visitorGroups.ForEach(exporter.AddVisitorGroup);
            exporter.Export(stream, sources, options);
            ((DefaultDataExporter)exporter)?.Close();
            stream.Position = 0;
            return stream;
        }
    }
}