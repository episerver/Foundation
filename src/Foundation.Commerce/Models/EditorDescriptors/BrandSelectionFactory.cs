using EPiServer;
using EPiServer.Commerce.Catalog.ContentTypes;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Mediachase.Commerce.Catalog;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Foundation.Commerce.Models.EditorDescriptors
{
    public class BrandSelectionFactory : ISelectionFactory
    {
        public virtual IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
            var referenceConverter = ServiceLocator.Current.GetInstance<ReferenceConverter>();

            var contentReferences = contentLoader.GetDescendents(referenceConverter.GetRootLink());

            if (contentReferences == null || !contentReferences.Any())
            {
                return new List<SelectItem>();
            }

            var entries = contentLoader.GetItems(contentReferences, CultureInfo.CurrentUICulture);

            var brands = entries
                .Where(x => (x as EntryContentBase) != null
                            && ((EntryContentBase)x).Property.Keys.Contains("Brand")
                            && ((EntryContentBase)x).Property["Brand"]?.Value?.ToString() != null)
                .Select(x => ((EntryContentBase)x).Property["Brand"]?.Value?.ToString())
                .Distinct();
            var items = brands.Select(x => new SelectItem { Text = x, Value = x }).OrderBy(i => i.Text).ToList();
            return items;
        }
    }
}