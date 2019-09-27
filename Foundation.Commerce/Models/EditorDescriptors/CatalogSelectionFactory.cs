using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Mediachase.Commerce.Catalog;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Commerce.Models.EditorDescriptors
{
    public class CatalogSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var contentRepository = ServiceLocator.Current.GetInstance<IContentRepository>();
            var referenceConverter = ServiceLocator.Current.GetInstance<ReferenceConverter>();

            var catalogs = contentRepository.GetChildren<CatalogContentBase>(referenceConverter.GetRootLink());
            var items = catalogs.Select(x => new SelectItem { Text = x.Name, Value = x.CatalogId }).ToList();
            items.Insert(0, new SelectItem { Text = "All", Value = 0 });
            return items;
        }
    }
}