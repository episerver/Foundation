using EPiServer.ServiceLocation;
using EPiServer.Shell.ObjectEditing;
using Foundation.Cms.Settings;
using Foundation.Features.Settings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Foundation.Features.People
{
    public class LocationsSelectionFactory : ISelectionFactory
    {
        private static readonly Lazy<ISettingsService> _settingsService = new Lazy<ISettingsService>(() => ServiceLocator.Current.GetInstance<ISettingsService>());

        public IEnumerable<ISelectItem> GetSelections(ExtendedMetadata metadata)
        {
            var settings = _settingsService.Value.GetSiteSettings<CollectionSettings>();
            return settings.Locations?.Select(x => new SelectItem { Value = x.Value, Text = x.Text }) ?? new List<SelectItem>(); ;
        }
    }
}
