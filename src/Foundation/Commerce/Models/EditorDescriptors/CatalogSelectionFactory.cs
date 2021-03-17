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
        private readonly Injected<IContentRepository> _contentRepository;
        private readonly Injected<ReferenceConverter> _referenceConverter;

        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var catalogs = _contentRepository.Service.GetChildren<CatalogContentBase>(_referenceConverter.Service.GetRootLink());
            var items = catalogs.Select(x => new SelectItem { Text = x.Name, Value = x.CatalogId }).ToList();
            items.Insert(0, new SelectItem { Text = "All", Value = 0 });
            return items;
        }
    }
}