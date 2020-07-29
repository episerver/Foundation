using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.Core;
using EPiServer.PlugIn;
using EPiServer.Scheduler;
using EPiServer.Security;
using Foundation.Features.CatalogContent;
using Mediachase.Commerce.Catalog;
using Mediachase.Commerce.Inventory;
using Mediachase.Commerce.InventoryService;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;

namespace Foundation.Infrastructure.Jobs
{
    [ScheduledPlugIn(DisplayName = "Demo Integration Job", GUID = "6DDF8B1A-2BAE-4492-AB21-777C70634D9F")]
    public class DemoIntegrationJob : ScheduledJobBase
    {
        private bool _stopSignaled;
        private readonly IInventoryService _inventoryService;
        private readonly IWarehouseRepository _warehouseRepository;
        private readonly IContentRepository _contentRepository;
        private readonly ReferenceConverter _referenceConverter;

        public DemoIntegrationJob(IInventoryService inventoryService,
            IWarehouseRepository warehouseRepository, IContentRepository contentRepository, ReferenceConverter referenceConverter)
        {
            _inventoryService = inventoryService;
            _warehouseRepository = warehouseRepository;
            IsStoppable = true;
            _contentRepository = contentRepository;
            _referenceConverter = referenceConverter;
        }

        /// <summary>
        /// Called when a user clicks on Stop for a manually started job, or when ASP.NET shuts down.
        /// </summary>
        public override void Stop() => _stopSignaled = true;

        /// <summary>
        /// Called when a scheduled job executes
        /// </summary>
        /// <returns>A status message to be stored in the database log and visible from admin mode</returns>
        public override string Execute()
        {
            //Call OnStatusChanged to periodically notify progress of job for manually started jobs
            OnStatusChanged(string.Format("Starting execution of {0}", GetType()));

            //Add implementation
            UpdateAllCatalogContent();
            //For long running jobs periodically check if stop is signaled and if so stop execution
            if (_stopSignaled)
            {
                return "Stop of job was called";
            }

            return "Change to message that describes outcome of execution";
        }

        private void UpdateAllCatalogContent()
        {
            foreach (var catalog in _contentRepository.GetChildren<CatalogContent>(_referenceConverter.GetRootLink()))
            {
                UpdateCatalogContentRecursive(catalog.ContentLink, new CultureInfo("en"));
            }
        }

        private void UpdateCatalogContentRecursive(ContentReference parentLink, CultureInfo defaultCulture)
        {
            foreach (var child in LoadChildrenBatched(parentLink, defaultCulture))
            {
                ((IProductRecommendations)child).ShowRecommendations = true;
                _contentRepository.Save(child, EPiServer.DataAccess.SaveAction.Publish, AccessLevel.NoAccess);

                UpdateCatalogContentRecursive(child.ContentLink, defaultCulture);
            }
        }

        private IEnumerable<CatalogContentBase> LoadChildrenBatched(ContentReference parentLink, CultureInfo defaultCulture)
        {
            var start = 0;

            while (true)
            {
                var batch = _contentRepository.GetChildren<CatalogContentBase>(parentLink, defaultCulture, start, 100)
                    .Where(x => x is IProductRecommendations)
                    .Select(x => x.CreateWritableClone());

                if (!batch.Any())
                {
                    yield break;
                }

                foreach (var content in batch)
                {
                    // Don't include linked products to avoid including them multiple times when traversing the catalog
                    if (!parentLink.CompareToIgnoreWorkID(content.ParentLink))
                    {
                        continue;
                    }

                    yield return content;
                }
                start += 50;
            }
        }
    }
}
