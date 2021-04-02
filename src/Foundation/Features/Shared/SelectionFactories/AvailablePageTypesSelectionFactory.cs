using EPiServer.DataAbstraction;
using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.Shared.SelectionFactories
{
    public class AvailablePageTypesSelectionFactory : ISelectionFactory
    {
        // Only support parameterless constructor
        private readonly Injected<IContentTypeRepository> _contentTypeRepository;

        private Dictionary<string, string> GetAvailablePageTypes()
        {
            var pageTypes = _contentTypeRepository.Service.List().OfType<PageType>();
            var availablePageTypes = pageTypes
                .Where(x => x.IsAvailable)
                .OrderBy(x => x.GroupName)
                .ThenBy(x => x.DisplayName)
                .Select(x =>
                {
                    return new KeyValuePair<string, string>(x.ID.ToString(), "[" + x.GroupName + "] " + x.Name);
                });

            return availablePageTypes.ToDictionary(x => x.Key, x => x.Value);
        }

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var availablePageTypes = GetAvailablePageTypes();
            return availablePageTypes.Select(x => new SelectItem { Value = x.Key, Text = x.Value });
        }
    }
}
